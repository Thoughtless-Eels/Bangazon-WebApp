﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BangazonWebApp.Data;
using BangazonWebApp.Models;
using BangazonWebApp.Models.ProductTypeViewModels;

namespace BangazonWebApp.Controllers
{
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductTypes
        public async Task<IActionResult> Index()
        {
           // return View(await _context.ProductType.ToListAsync());
            var model = new List<ProductTypeDisplayModel>();

            // Build list of Product instances for display in view
            // LINQ is awesome
            model = await (
                from t in _context.ProductType
                join p in _context.Product
                on t.ProductTypeId equals p.ProductTypeId
                group new { t, p } by new { t.ProductTypeId, t.ProductTypeName } into grouped
                select new ProductTypeDisplayModel
                {
                    ProductTypeId = grouped.Key.ProductTypeId,
                    ProductTypeName = grouped.Key.ProductTypeName,
                    ProductCount = grouped.Select(x => x.p.ProductId).Count(),
                    CategoryProducts = grouped.Select(x => x.p).Take(3).ToList()
                }).ToListAsync();

            return View(model);


        }

        // GET: ProductTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType
                .SingleOrDefaultAsync(m => m.ProductTypeId == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        //Types from Steves code
        public async Task<IActionResult> Types()
        {
            var model = new List<ProductTypeDisplayModel>();

            // Build list of Product instances for display in view
            // LINQ is awesome
            model = await (
                from t in _context.ProductType
                join p in _context.Product
                on t.ProductTypeId equals p.ProductTypeId
                group new { t, p } by new { t.ProductTypeId, t.ProductTypeName } into grouped
                select new ProductTypeDisplayModel
                {
                    ProductTypeId = grouped.Key.ProductTypeId,
                    ProductTypeName = grouped.Key.ProductTypeName,
                    ProductCount = grouped.Select(x => x.p.ProductId).Count(),
                    CategoryProducts = grouped.Select(x => x.p).Take(3).ToList()
                }).ToListAsync();

            return View(model);
        }
        //Steves code ends here

        // GET: ProductTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductTypeId,ProductTypeName")] ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        // GET: ProductTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType.SingleOrDefaultAsync(m => m.ProductTypeId == id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        // POST: ProductTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductTypeId,ProductTypeName")] ProductType productType)
        {
            if (id != productType.ProductTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTypeExists(productType.ProductTypeId))
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
            return View(productType);
        }

        // GET: ProductTypes/Delete/5
       // public async Task<IActionResult> Delete(int? id)
       // {
         //   if (id == null)
           // {
              //  return NotFound();
            //}

            //var productType = await _context.ProductType
              //  .SingleOrDefaultAsync(m => m.ProductTypeId == id);
            //if (productType == null)
            //{
              //  return NotFound();
            //}

            //return View(productType);
        //}

        // POST: ProductTypes/Delete/5
       [HttpPost, ActionName("Delete")]
       [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productType = await _context.ProductType.SingleOrDefaultAsync(m => m.ProductTypeId == id);
            _context.ProductType.Remove(productType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTypeExists(int id)
        {
            return _context.ProductType.Any(e => e.ProductTypeId == id);
     }
    }
}
