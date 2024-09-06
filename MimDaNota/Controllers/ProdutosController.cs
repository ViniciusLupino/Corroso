using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimDaNota.Data;
using MimDaNota.Models;

namespace MimDaNota.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly Context _context;

        public ProdutosController(Context context)
        {
            _context = context;
        }

        // GET: Produto
        public async Task<IActionResult> Index()
        {
            var context = _context.Produto.Include(p => p.Categoria).Include(p => p.Fornecedor).Include(p => p.User);
            return View(await context.ToListAsync());
        }

        // GET: Produto/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produto/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId");
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "FornecedorId", "FornecedorId");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: Produto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProdutoId,ProdutoDescricao,ProdutoNome,ProdutoPreco,ProdutoEstoque,UserId,FornecedorId,CategoriaId")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                produto.ProdutoId = Guid.NewGuid();
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", produto.CategoriaId);
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "FornecedorId", "FornecedorId", produto.FornecedorId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", produto.UserId);
            return View(produto);
        }

        // GET: Produto/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", produto.CategoriaId);
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "FornecedorId", "FornecedorId", produto.FornecedorId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", produto.UserId);
            return View(produto);
        }

        // POST: Produto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProdutoId,ProdutoDescricao,ProdutoNome,ProdutoPreco,ProdutoEstoque,UserId,FornecedorId,CategoriaId")] Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.ProdutoId))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", produto.CategoriaId);
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "FornecedorId", "FornecedorId", produto.FornecedorId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", produto.UserId);
            return View(produto);
        }

        // GET: Produto/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto != null)
            {
                _context.Produto.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(Guid id)
        {
            return _context.Produto.Any(e => e.ProdutoId == id);
        }
    }
}
