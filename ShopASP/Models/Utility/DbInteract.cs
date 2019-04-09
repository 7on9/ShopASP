using ShopASP.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopASP.Models.Utility
{
    public static class DbInteract
    {
        public static Product GetProduct(int id)
        {
            dbShopASPDataContext db = new dbShopASPDataContext();
            var productFromDb = (from a in db.products
                                 join b in db.product_details
                                  on a.product_id equals b.product_id
                                 join c in db.product_imgs
                                  on a.product_id equals c.product_id
                                 join d in db.colors
                                  on c.color_id equals d.color_id
                                 where a.product_id == id
                                 select new
                                 {
                                     a.product_id,
                                     a.product_quantum,
                                     a.product_price,
                                     b.product_decrible,
                                     b.product_tag,
                                     c.color_id,
                                     d.color_name,
                                     d.color_hex,
                                     b.product_name,
                                     c.product_img_path
                                 }).ToList();
            Product product = new Product();

            product.Id = productFromDb[0].product_id;
            product.Price = (float)productFromDb[0].product_price;
            product.Quantum = (int)productFromDb[0].product_quantum;
            product.Tag = productFromDb[0].product_tag;
            product.Name = productFromDb[0].product_name;
            product.Describle = productFromDb[0].product_decrible;

            foreach (var i in productFromDb)
            {
                product.ImagePaths = new ProductImg(i.product_id, i.product_img_path, i.color_id);
                product.Colors = new Color(i.color_id, i.color_name, i.color_hex);
            }
            return product;
        }

        public static List<Product> GetAllProducts()
        {
            dbShopASPDataContext db = new dbShopASPDataContext();
            var listProducts = (from a in db.products
                                join b in db.product_details
                                 on a.product_id equals b.product_id
                                join c in db.product_imgs
                                 on a.product_id equals c.product_id
                                where a.product_quantum >= 0
                                select new
                                {
                                    a.product_id,
                                    a.product_price,
                                    b.product_name,
                                    c.product_img_path,
                                    c.color_id
                                }).ToList();
            List<Product> products = new List<Product>();
            int i = 0;
            foreach (var product in listProducts)
            {
                products.Add(new Product());
                products[i].Id = product.product_id;
                products[i].Name = product.product_name;
                products[i].ImagePaths = new ProductImg(product.product_id, product.product_img_path, product.color_id);
                products[i].Price = (float)product.product_price;
                i++;
            }
            return products;
            //return dbShopASP.products.ToList();
        }

        public static List<Cart> GetAllWaitingCart()
        {
            dbShopASPDataContext db = new dbShopASPDataContext();
            var listCart = (from a in db.carts
                            where a.cart_status.Equals(false)
                            select new
                            {
                                a.cart_id,
                                a.customer_id,
                                a.time_create,
                                a.cart_status
                            }).ToList();

            List<Cart> carts = new List<Cart>();
            int i = 0;
            foreach (var cart in listCart)
            {
                carts.Add(new Cart());
                carts[i].Id = cart.cart_id;
                carts[i].Customer = GetCustomerById(cart.customer_id);
                carts[i].TimeCreate = cart.time_create.ToString();
                carts[i].Status = (bool)cart.cart_status;
                i++;
            }

            return carts;
        }

        public static Customer GetCustomerById(int id)
        {
            dbShopASPDataContext db = new dbShopASPDataContext();
            var customer = db
                .customers
                .SingleOrDefault
                (
                    n => (n.customer_id == id)
                );
            Customer returnCustomer = new Customer();
            if (customer != null)
            {
                returnCustomer.ID = customer.customer_id;
                returnCustomer.Dob = customer.customer_dob;
                returnCustomer.Gender = (bool)customer.customer_gender;
                returnCustomer.Phone = customer.customer_phone;
                returnCustomer.Address = customer.customer_address;
                returnCustomer.Email = customer.customer_email;
                returnCustomer.Name = customer.customer_name;

            }
            return returnCustomer;
        }

        public static Cart GetFullDetailOfCart(int id)
        {
            dbShopASPDataContext db = new dbShopASPDataContext();
            var cartFormDb = (from a in db.carts
                              where a.cart_id.Equals(id)
                              select new
                              {
                                  a.cart_id,
                                  a.customer_id,
                                  a.time_create,
                                  a.cart_status
                              }).ToList();

            Cart cart = new Cart();
            cart.Id = cartFormDb[0].cart_id;
            cart.Customer = GetCustomerById(cartFormDb[0].customer_id);
            cart.TimeCreate = cartFormDb[0].time_create.ToString();
            cart.Status = (bool)cartFormDb[0].cart_status;
            cart.Product = new List<CartItem>();
            var products = (from a in db.cart_details
                            where a.cart_id.Equals(cart.Id)
                            select new
                            {
                                a.cart_id,
                                a.product_id,
                                a.quantum,
                                a.color_id,
                                a.size_id
                            }).ToList();
            foreach (var product in products)
            {
                cart.Product.Add(
                    new CartItem(GetProduct(product.product_id),
                    product.quantum,
                    product.color_id,
                    product.size_id));
            }
            return cart;
        }

        public static float GetTotalOfCart(int id)
        {
            Cart cart = GetFullDetailOfCart(id);
            float total = 0;
            foreach(var product in cart.Product)
            {
                total += (product.Product.Price * product.Quantity);
            }
            total += 20 + total / 10;
            return total;
        }

        public static bool CreateBill(int cart_id, int employee_id)
        {
            dbShopASPDataContext db = new dbShopASPDataContext();
            try
            {
                db.ExecuteQuery<bill>("Insert into bill values({0}, {1}, {2})", employee_id, cart_id, Utility.GetNowToSQLDateTime());
                db.SubmitChanges();
            }
            catch
            {

            }
            
            return true;
        }

        public static List<Product> GetTopProducts(int count)
        {
            dbShopASPDataContext db = new dbShopASPDataContext();
            var listProducts = (from a in db.products
                                join b in db.product_details
                                 on a.product_id equals b.product_id
                                join c in db.product_imgs
                                 on a.product_id equals c.product_id
                                 where a.product_quantum >= 0
                                select new
                                {
                                    a.product_id,
                                    a.product_price,
                                    b.product_name,
                                    c.product_img_path,
                                    c.color_id
                                }).Take(count).ToList();
            List<Product> products = new List<Product>();
            int i = 0;
            foreach (var product in listProducts)
            {
                products.Add(new Product());
                products[i].Id = product.product_id;
                products[i].Name = product.product_name;
                products[i].ImagePaths = new ProductImg(product.product_id, product.product_img_path, product.color_id);
                products[i].Price = (float)product.product_price;
                i++;
            }
            return products;
        }

        public static Color GetColorById(int id)
        {
            dbShopASPDataContext db = new dbShopASPDataContext();
            var color = db.colors.FirstOrDefault(m => m.color_id.Equals(id));
            Color returnColor = new Color(id, color.color_name, color.color_hex);
            return returnColor;
        }

        public static size GetSizeById(int id)
        {
            dbShopASPDataContext db = new dbShopASPDataContext();
            return db.sizes.FirstOrDefault(m => m.size_id.Equals(id));
        }
    }
}