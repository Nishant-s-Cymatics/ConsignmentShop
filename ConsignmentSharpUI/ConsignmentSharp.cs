using ConsignmentSharpLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentSharpUI
{
    public partial class ConsignmentSharp : Form
    {
        private Store store = new Store();
        private List<Item> shoppingCartData = new List<Item>();
        BindingSource itemsBinding = new BindingSource();
        BindingSource cartBinding = new BindingSource();
        BindingSource vendorsBinding = new BindingSource();

        private decimal storeprofit = 0;


        public ConsignmentSharp()
        {
            InitializeComponent();
            SetupData();

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();
            itemsListBox.DataSource = itemsBinding;

            itemsListBox.DisplayMember = "Display";
            itemsListBox.ValueMember = "Display";

            cartBinding.DataSource = shoppingCartData;
            shoppingCartListBox.DataSource = cartBinding;

            shoppingCartListBox.DisplayMember = "Display";
            shoppingCartListBox.ValueMember = "Display";

            vendorsBinding.DataSource = store.Vendors;
            vendorListBox.DataSource = vendorsBinding;

            vendorListBox.DisplayMember = "Display";
            vendorListBox.ValueMember = "Display";
        }

        private void SetupData()
        {

            store.Vendors.Add(new Vendor { FirstName = "Nishant", LastName = "Navade" });
            store.Vendors.Add(new Vendor { FirstName = "Jai", LastName = "Navade" });

            store.Items.Add(new Item
            {
                Tilte = "Shantaram",
                Description = "A book",
                Price = 10M,
                Owner = store.Vendors[0]
            });

            store.Items.Add(new Item
            {
                Tilte = "Wonder",
                Description = "An autobiography",
                Price = 20M,
                Owner = store.Vendors[1]
            });

            store.Items.Add(new Item
            {
                Tilte = "harry porter",
                Description = "A mystery book",
                Price = 15.56M,
                Owner = store.Vendors[0]
            });

            store.Items.Add(new Item
            {
                Tilte = "Jane Austin",
                Description = "A book abt a boy",
                Price = 50M,
                Owner = store.Vendors[1]
            });

            store.Name = "Sapna";
        }

        private void addToCart_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("I have been clicked");
            //This action button should:
            //1. Figure out what is selected from the items list
            //2. Copy that item to shopping cart
            //3. Do we remove the item from items list? - NO

            Item selectedItem = (Item)itemsListBox.SelectedItem;
            //MessageBox.Show(selectedItem.Tilte);

            shoppingCartData.Add(selectedItem);
            cartBinding.ResetBindings(false);

        }

        private void purchase_Click(object sender, EventArgs e)
        {
            //Mark each item in the car as sold
            //Clear the cart

            foreach(Item item in shoppingCartData)
            {
                item.Sold = true;
                item.Owner.PaymentDue += (decimal)item.Owner.Commission * item.Price;
                storeprofit += (1 - (decimal)item.Owner.Commission) * item.Price;
            }

            shoppingCartData.Clear();

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();

            storeProfitValue.Text = string.Format("${0}", storeprofit);

            cartBinding.ResetBindings(false);
            itemsBinding.ResetBindings(false);
            vendorsBinding.ResetBindings(false);

        }
    }
}
