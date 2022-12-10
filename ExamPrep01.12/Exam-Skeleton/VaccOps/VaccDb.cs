namespace VaccOps
{
    using Models;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VaccDb : IVaccOps
    {
        private Dictionary<string, Doctor> doctorsByName = new Dictionary<string, Doctor>();
        private Dictionary<string, Patient> patientsByName = new Dictionary<string, Patient>();

        public void AddDoctor(Doctor doctor)
        {
            if (this.doctorsByName.ContainsKey(doctor.Name))
            {
                throw new ArgumentException();
            }

            this.doctorsByName.Add(doctor.Name, doctor);
        }

        public void AddPatient(Doctor doctor, Patient patient)
        {
            if (!this.doctorsByName.ContainsKey(doctor.Name))
            {
                throw new ArgumentException();
            }

            patientsByName.Add(patient.Name, patient);

            this.doctorsByName[doctor.Name].Patients.Add(patient);

            patient.Doctor = doctor;
        }

        public void ChangeDoctor(Doctor oldDoctor, Doctor newDoctor, Patient patient)
        {
            if (!doctorsByName.ContainsKey(oldDoctor.Name) 
                || !doctorsByName.ContainsKey(newDoctor.Name)
                || !patientsByName.ContainsKey(patient.Name))
            {
                throw new ArgumentException();
            }

            oldDoctor.Patients.Remove(patient);
            newDoctor.Patients.Add(patient);
            patient.Doctor = newDoctor;
        }

        public bool Exist(Doctor doctor)
        {
            return this.doctorsByName.ContainsKey(doctor.Name);
        }

        public bool Exist(Patient patient)
        {
            return this.patientsByName.ContainsKey(patient.Name);
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return this.doctorsByName.Values;
        }

        public IEnumerable<Doctor> GetDoctorsByPopularity(int populariry)
        {
            return this.doctorsByName.Values.Where(x => x.Popularity == populariry);
        }

        public IEnumerable<Doctor> GetDoctorsSortedByPatientsCountDescAndNameAsc()
        {
            return this.GetDoctors()
                    .OrderByDescending(d => d.Patients.Count)
                    .ThenBy(d => d.Name);
        }

        public IEnumerable<Patient> GetPatients()
        {
            return this.patientsByName.Values;
        }

        public IEnumerable<Patient> GetPatientsByTown(string town)
        {
            return this.patientsByName.Values.Where(p => p.Town == town);
        }

        public IEnumerable<Patient> GetPatientsInAgeRange(int lo, int hi)
        {
            return this.patientsByName.Values.Where(p => p.Age >= lo && p.Age <= hi);
        }

        public IEnumerable<Patient> GetPatientsSortedByDoctorsPopularityAscThenByHeightDescThenByAge()
        {
            return this.GetPatients()
                    .OrderBy(p => p.Doctor.Popularity)
                    .ThenByDescending(p => p.Height)
                    .ThenBy(p => p.Age);
        }

        public Doctor RemoveDoctor(string name)
        {
            if (!this.doctorsByName.ContainsKey(name))
            {
                throw new ArgumentException();
            }

            var doctor = this.doctorsByName[name];

            this.doctorsByName.Remove(name);

            foreach (var patient in doctor.Patients)
            {
                this.patientsByName.Remove(patient.Name);
            }

            return doctor;
        }
    }
}
