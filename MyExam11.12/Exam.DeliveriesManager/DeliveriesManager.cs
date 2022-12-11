using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.DeliveriesManager
{
    public class DeliveriesManager : IDeliveriesManager
    {
        private Dictionary<string, Deliverer> deliverersById = new Dictionary<string, Deliverer>();
        private Dictionary<string, Package> packagesById = new Dictionary<string, Package>();

        public void AddDeliverer(Deliverer deliverer)
        {
            if (this.deliverersById.ContainsKey(deliverer.Id))
            {
                throw new ArgumentException();
            }
            
            this.deliverersById.Add(deliverer.Id, deliverer);
        }

        public void AddPackage(Package package)
        {
            if (this.packagesById.ContainsKey(package.Id))
            {
                throw new ArgumentException();
            }
            
            this.packagesById.Add(package.Id, package);
        }

        public void AssignPackage(Deliverer deliverer, Package package)
        {
            if (!this.deliverersById.ContainsKey(deliverer.Id))
            {
                throw new ArgumentException();
            }

            if (!this.packagesById.ContainsKey(package.Id))
            {
                throw new ArgumentException();
            }

            this.deliverersById[deliverer.Id].Packages.Add(package);
            package.Deliverer = deliverer;
        }

        public bool Contains(Deliverer deliverer)
        {
            return this.deliverersById.ContainsKey(deliverer.Id);
        }

        public bool Contains(Package package)
        {
            return this.packagesById.ContainsKey(package.Id);
        }

        public IEnumerable<Deliverer> GetDeliverers()
        {
            return this.deliverersById.Values;
        }

        public IEnumerable<Deliverer> GetDeliverersOrderedByCountOfPackagesThenByName()
        {
            return this.deliverersById.Values.OrderByDescending(d => d.Packages.Count).ThenBy(d => d.Name);
        }

        public IEnumerable<Package> GetPackages()
        {
            return this.packagesById.Values;
        }

        public IEnumerable<Package> GetPackagesOrderedByWeightThenByReceiver()
        {
            return this.packagesById.Values.OrderByDescending(p => p.Weight).ThenBy(p => p.Receiver);
        }

        public IEnumerable<Package> GetUnassignedPackages()
        {
            return this.packagesById.Values.Where(p => p.Deliverer == null);
        }
    }
}
