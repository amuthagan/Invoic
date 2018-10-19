using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.Model;
using ProcessDesigner.BLL;


namespace ProcessDesigner.ViewModel
{
    class ProductCategoryViewModel : BindableBase
    {
        private ProductCategoryModel productCategory;
        private ProductCategoryBll productCategoryBll;
        public ProductCategoryViewModel()
        {
            productCategory = new ProductCategoryModel();
            productCategoryBll = new ProductCategoryBll();
            productCategoryBll.GetProductCategory(ProductCategory);
        }

        public ProductCategoryModel ProductCategory
        {
            get { return this.productCategory; }
            set { SetProperty(ref this.productCategory, value); }
        }
    }
}
