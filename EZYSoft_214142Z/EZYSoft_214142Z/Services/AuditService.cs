using EZYSoft_214142Z.Model;
using Fresh_Farm_Market_214142Z.Model;

namespace Fresh_Farm_Market_214142Z.Services
{
    public class AuditService
    {
        private readonly AuthDbContext _context;

        public AuditService(AuthDbContext context)
        {
            _context = context;
        }

        public List<Audit> GetAll()
        {
            return _context.AuditDb.ToList();
        }


        public void AddAudit(Audit audit)
        {
            _context.AuditDb.Add(audit);
            _context.SaveChanges();
        }

        public void UpdateAudit(Audit audit)
        {
            _context.AuditDb.Update(audit);
            _context.SaveChanges();
        }

    }

}
