using CDoctor.Web.Data;
using CDoctor.Web.Models;
using CDoctor.Web.Utility;
using CDoctor.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDoctor.Web.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddUpdate(AppointmentViewModel model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

            if (model != null && model.Id > 0)
            {
                return 1;
            }
            else
            {
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    IsAppointmentApproved = false,
                    AdminId = model.AdminId
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return 2;
            }
        }

        public List<DoctorViewModel> GetDoctorList()
        {
            var doctors = (from user in _context.Users
                           join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
                           join roles in _context.Roles.Where(u => u.Name == Helper.Doctor) on userRoles.RoleId equals roles.Id
                           select new DoctorViewModel
                           {
                               Id = user.Id,
                               Name = user.Name
                           }
                           ).ToList();

            //var doctors = _context.Users.ToList();

            return doctors;
        }

        public List<PatientViewModel> GetPatientList()
        {
            var patients = (from user in _context.Users
                           join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
                           join roles in _context.Roles.Where(u => u.Name == Helper.Patient) on userRoles.RoleId equals roles.Id
                           select new PatientViewModel
                           {
                               Id = user.Id,
                               Name = user.Name
                           }
               ).ToList();
            return patients;
        }
    }
}
