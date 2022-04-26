using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WETT.Data;

namespace WETT.Models
{
    public class SupplierViewModel
    {
        private IList<Supplier> _suppliers;

        public IList<Supplier> SuppliersDatabase
        {
            get
            {
                //  if (_suppliers == null) _suppliers = JsonConvert.DeserializeObject<List<Supplier>>(File.ReadAllText(@"jsondata\suppliers.json"));
                if (_suppliers == null) _suppliers = new List<Supplier>();
               // if (_claims == null) _claims = new List<Claim>();

                return _suppliers;
            }
        }

        internal void UpdateSupplier(Supplier supp)
        {
            _suppliers = SuppliersDatabase;

            Supplier s = _suppliers.SingleOrDefault(s => s.SupplierCode.Equals(supp.SupplierCode));

            if (s != null)
                _suppliers.Remove(s);
            else
                supp.SupplierCode = _suppliers.Max(m => m.SupplierCode) + 1;

            _suppliers.Add(supp);

            File.WriteAllText(@"jsondata\suppliers.json", JsonConvert.SerializeObject(_suppliers, Formatting.Indented));
        }

        internal void DeleteSupplier(string id)
        {
            _suppliers = SuppliersDatabase;

            _suppliers.Remove(_suppliers.Single(s => s.SupplierCode.ToString().Equals(id)));

            File.WriteAllText(@"jsondata\suppliers.json", JsonConvert.SerializeObject(_suppliers, Formatting.Indented));
        }
    }
}
