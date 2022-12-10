namespace CouponOps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CouponOps.Models;
    using Interfaces;

    public class CouponOperations : ICouponOperations
    {
        private Dictionary<string, Coupon> couponsByCode = new Dictionary<string, Coupon>();
        private Dictionary<string, Website> websitesByDomain = new Dictionary<string, Website>();

        public void AddCoupon(Website website, Coupon coupon)
        {
            if (!this.websitesByDomain.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            this.websitesByDomain[website.Domain].Coupons.Add(coupon);
            coupon.Website = website;
            this.couponsByCode.Add(coupon.Code, coupon);
        }

        public bool Exist(Website website)
        {
            return this.websitesByDomain.ContainsKey(website.Domain);
        }

        public bool Exist(Coupon coupon)
        {
            return this.couponsByCode.ContainsKey(coupon.Code);
        }

        public IEnumerable<Coupon> GetCouponsForWebsite(Website website)
        {
            if (!this.websitesByDomain.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            return this.websitesByDomain[website.Domain].Coupons;
        }

        public IEnumerable<Coupon> GetCouponsOrderedByValidityDescAndDiscountPercentageDesc()
        {
            return this.couponsByCode.Values.OrderByDescending(c => c.Validity).ThenByDescending(c => c.DiscountPercentage);
        }

        public IEnumerable<Website> GetSites()
        {
            return this.websitesByDomain.Values;
        }

        public IEnumerable<Website> GetWebsitesOrderedByUserCountAndCouponsCountDesc()
        {
            return this.websitesByDomain.Values.OrderBy(w => w.UsersCount).ThenByDescending(w => w.Coupons.Count);
        }

        public void RegisterSite(Website website)
        {
            if (this.websitesByDomain.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            this.websitesByDomain.Add(website.Domain, website);
        }

        public Coupon RemoveCoupon(string code)
        {
            if (!this.couponsByCode.ContainsKey(code))
            {
                throw new ArgumentException();
            }

            var coupon = this.couponsByCode[code];
            var couponWebsite = coupon.Website;

            this.websitesByDomain[couponWebsite.Domain].Coupons.Remove(coupon);

            this.couponsByCode.Remove(code);

            return coupon;
        }

        public Website RemoveWebsite(string domain)
        {
            if (!this.websitesByDomain.ContainsKey(domain))
            {
                throw new ArgumentException();
            }

            var website = this.websitesByDomain[domain];

            this.websitesByDomain.Remove(domain);

            foreach (var coupon in website.Coupons)
            {
                this.couponsByCode.Remove(coupon.Code);
            }

            return website;
        }

        public void UseCoupon(Website website, Coupon coupon)
        {
            if (!this.websitesByDomain.ContainsKey(website.Domain))
            {
                throw new ArgumentException();
            }

            if (!this.couponsByCode.ContainsKey(coupon.Code))
            {
                throw new ArgumentException();
            }

            if (coupon.Website.Domain != website.Domain)
            {
                throw new ArgumentException();
            }

            if (!this.websitesByDomain[website.Domain].Coupons.Contains(coupon))
            {
                throw new ArgumentException();
            }

            this.websitesByDomain[website.Domain].Coupons.Remove(coupon);
            this.couponsByCode.Remove(coupon.Code);
        }
    }
}