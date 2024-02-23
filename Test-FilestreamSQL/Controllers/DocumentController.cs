using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Test_FilestreamSQL.Models;
using Test_FilestreamSQL.Models.DAL;
using Test_FilestreamSQL.Models.Support;

namespace Test_FilestreamSQL.Controllers
{
    [ApiController]
    [Route("/api/")]
    public class DocumentController : Controller
    {
        private readonly DBContext _context;
        private const string RowDataStatement = @"SELECT Data.PathName() AS 'Path', GET_FILESTREAM_TRANSACTION_CONTEXT() AS 'Transaction' FROM Attachments WHERE Id = @id";
        public IConfiguration _configuration { get; }
        public DocumentController(DBContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }
        [HttpGet("GetAttachment")]
        public Attachment GetById(int id)
        {
            var _attachment = _context.Attachments.FirstOrDefault(p => p.Id == id);
            if (_attachment == null)
            {
                return null;
            }
            using (var tx = new TransactionScope())
            {
                var rowData = _context.Database.SqlQueryRaw<FileStreamRowData>(RowDataStatement, new SqlParameter("id", id)).First();
                using (var source = new SqlFileStream(rowData.Path, rowData.Transaction, FileAccess.Read))
                {
                    var buffer = new byte[16 * 1024];
                    using (var ms = new MemoryStream())
                    {
                        int bytesRead;
                        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, bytesRead);
                        }
                        _attachment.Data = ms.ToArray();
                    }
                }
                tx.Complete();
            }
            return _attachment;
        }

        [HttpPost("SaveAttachment")]
        public void Insert(Attachment entity)
        {
            using (var tx = new TransactionScope())
            {
                _context.Add(entity);
                _context.SaveChanges();
                SaveData(_context, entity);
                tx.Complete();
            }
        }
        private static void SaveData(DBContext context, Attachment entity)
        {
            var rowData = context.Database.SqlQueryRaw<FileStreamRowData>(RowDataStatement, new SqlParameter("id", entity.Id)).FirstOrDefault();
            using (var destination = new SqlFileStream(rowData.Path, rowData.Transaction, FileAccess.Write))
            {
                var buffer = new byte[16 * 1024];
                using (var ms = new MemoryStream(entity.Data))
                {
                    int bytesRead;
                    while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        destination.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
    }
}
