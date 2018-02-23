using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BangazonWebApp.Data;
using BangazonWebApp.Models;
using BangazonWebApp.Models.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BangazonWebApp.Models.ProductTypeViewModels;

// Author: Greg Lawrence
// Purpose: To handle actions dealing with Products

namespace BangazonWebApp.Controllers
{
    public class ProductsController : Controller
    {

        // create an instance of the UserManager to be able to retrieve the current active user
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _userManager = userManager;
        }

        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Products
        // Returns a list of all the products in the system
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Product.Include(p => p.ProductType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products added by current user
        // Returns products in the database that were created by the current user
        public async Task<IActionResult> MyProducts()
        {
            var user = await GetCurrentUserAsync();
            var applicationDbContext = _context.Product.Include(p => p.ProductType).Where(p => p.User == user);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        // Returns the details of a single product
        // Parameters: Int Id, this is id is for the product requested and is used to search the database and find the matching product
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // find the product requested
            var product = await _context.Product
                .Include(p => p.ProductType)
                .SingleOrDefaultAsync(m => m.ProductId == id);

            // find the amount times this product has sold and return the number
            var SoldAmount = (
                from o in _context.Order
                join li in _context.LineItem on o.OrderId equals li.OrderId
                join p in _context.Product on li.ProductId equals p.ProductId
                where o.PaymentType != null && p.ProductId == id
                group p by p.ProductId into inv
                select inv).ToList().Count;

            // use the amount sold number to calculate the available inventory for this product
            product.Quantity -= SoldAmount;

            // if the product can't be found, return Not Found()
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        // Returns the Create a Product view
        [Authorize]
        public async Task<IActionResult> Create()
        {

            CreateProductViewModel productViewModel = new CreateProductViewModel(_context);

            return View(productViewModel);

        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Returns the user to the List of Products View
        // Parameters: takes a Product Object with the values that were inserted into the form 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Quantity,Description,Price,LocalDelivery,Location,Photo,ProductTypeId")] Product product)
        {
            // remove the user validation check since we do not attach the user object to the form
            ModelState.Remove("product.User");

            // if all of the other fields validate on model, attach the current user to product and save to database
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();

                product.User = user;

                _context.Add(product);
                await _context.SaveChangesAsync();
                // redirect the user to the details view of the product they just added
                return RedirectToAction("Details", new { Id = product.ProductId });
            }
            // if the validation fails, display the create product view model
            CreateProductViewModel cpvm = new CreateProductViewModel(_context);
            return View(cpvm);
        }

        // GET: Products/Edit/5
        // Returns a view of a specific product with details
        // Paramters: Int Id of the specific Product to be edited
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "ProductTypeName", product.ProductTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Returns a view of a specific product with details
        // Paramters: Int Id of the specific Product to be edited
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Title,Quantity,DateCreated,Description,Price,LocalDelivery,Location,Photo,ProductTypeId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "ProductTypeName", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        // Method retrieved the product to be deleted from the database using the Id that is passed in
        // Parameters: Int Id to be used to find the product to delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .SingleOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        // Handles deleting the selected product from the database
        // Parameters: Int Id to be used to find the product to delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.SingleOrDefaultAsync(m => m.ProductId == id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
           
        // Method checks if a product exists in the database and returns a True or False value
        // Parameters: Int Id to be used to search for a matching product
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }

        
         
         //Method to search the database for a product. 
         //Parameters: String search that the user inputs into a search box on website. The method searches through Product table to find a match in the Product Title.     
           
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchForProduct(string search)
        {
            //return as 404 if search is null or not found in db
            if (search == null)
            {
                return NotFound();
            }
            //find any product that contain the searched value 
            var product = await _context.Product
                .Include(p => p.ProductType)
                .Where(m => m.Title.Contains(search))
                .ToListAsync();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /*
            Method to add a product to the shopping cart (LineItem join table)
        */
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddProductToCart([FromRoute] int id)
        {
            // Find the product requested
            Product productToAdd = await _context.Product.SingleOrDefaultAsync(p => p.ProductId == id);

            // Get the current user
            var user = await GetCurrentUserAsync();

            // Get open order, if exists, otherwise null
            Order openOrder = await _context.Order.SingleOrDefaultAsync(o => o.User == user && o.PaymentTypeId == null);

            // Didn't find an open order
            if (openOrder == null)
            {

                // Create new order
                Order newOrder = new Order();
                // add the current user to the order
                newOrder.User = user;
                // add the order to the db context file
                _context.Add(newOrder);

                // Create new line item
                LineItem li = new LineItem()
                {
                    // add the product Id and the order Id to the line item
                    ProductId = id,
                    OrderId = newOrder.OrderId
                };
                // add the lineitem to the db context file
                _context.Add(li);

            }
            else
                // Open order exists
            {

                // Create new line item
                LineItem li = new LineItem()
                {
                    // add the product Id and the order Id to the line item
                    ProductId = id,
                    OrderId = openOrder.OrderId
                };

                // Add line item to db context
                _context.Add(li);

            }

            // Save all items in the db context
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
