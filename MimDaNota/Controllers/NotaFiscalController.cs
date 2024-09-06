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
    public class NotaFiscalController : Controller
    {
        private readonly Context _context;

        public NotaFiscalController(Context context)
        {
            _context = context;
        }

        // GET: NotaFiscal
        public async Task<IActionResult> Index()
        {
            var context = _context.NotaFiscal.Include(n => n.Produto).Include(n => n.Usuario);
            return View(await context.ToListAsync());
        }

        // GET: NotaFiscal/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaFiscal = await _context.NotaFiscal
                .Include(n => n.Produto)
                .Include(n => n.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notaFiscal == null)
            {
                return NotFound();
            }

            return View(notaFiscal);
        }

        // GET: NotaFiscal/Create
        public IActionResult Create()
        {
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "ProdutoId");
            ViewData["UsuarioId"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: NotaFiscal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantidade,ProdutoId,UsuarioId")] NotaFiscal notaFiscal)
        {
            if (ModelState.IsValid)
            {
                // Verificar se há estoque suficiente
                var produto = await _context.Produto.FindAsync(notaFiscal.ProdutoId);
                if (produto == null || produto.ProdutoEstoque < notaFiscal.Quantidade)
                {
                    ModelState.AddModelError("", "Estoque insuficiente para completar a nota fiscal.");
                    ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "ProdutoId", notaFiscal.ProdutoId);
                    ViewData["UsuarioId"] = new SelectList(_context.User, "UserId", "UserId", notaFiscal.UsuarioId);
                    return View(notaFiscal);
                }

                // Atualizar o estoque
                produto.ProdutoEstoque -= notaFiscal.Quantidade;
                _context.Update(produto);
                 
                // Adicionar a nota fiscal
                notaFiscal.Id = Guid.NewGuid();
                _context.Add(notaFiscal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "ProdutoId", notaFiscal.ProdutoId);
            ViewData["UsuarioId"] = new SelectList(_context.User, "UserId", "UserId", notaFiscal.UsuarioId);
            return View(notaFiscal);
        }

        // GET: NotaFiscal/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaFiscal = await _context.NotaFiscal.FindAsync(id);
            if (notaFiscal == null)
            {
                return NotFound();
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "ProdutoId", notaFiscal.ProdutoId);
            ViewData["UsuarioId"] = new SelectList(_context.User, "UserId", "UserId", notaFiscal.UsuarioId);
            return View(notaFiscal);
        }

        // POST: NotaFiscal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Quantidade,ProdutoId,UsuarioId")] NotaFiscal notaFiscal)
        {
            if (id != notaFiscal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notaFiscal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaFiscalExists(notaFiscal.Id))
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
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "ProdutoId", notaFiscal.ProdutoId);
            ViewData["UsuarioId"] = new SelectList(_context.User, "UserId", "UserId", notaFiscal.UsuarioId);
            return View(notaFiscal);
        }

        // GET: NotaFiscal/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var notaFiscal = await _context.NotaFiscal.FindAsync(id);
            if (notaFiscal != null)
            {
                // Reverter o estoque
                var produto = await _context.Produto.FindAsync(notaFiscal.ProdutoId);
                if (produto != null)
                {
                    produto.ProdutoEstoque += notaFiscal.Quantidade;
                    _context.Update(produto);
                }

                _context.NotaFiscal.Remove(notaFiscal);
                await _context.SaveChangesAsync();
            }
            return View(notaFiscal);
        }

        // POST: NotaFiscal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var notaFiscal = await _context.NotaFiscal.FindAsync(id);
            if (notaFiscal != null)
            {
                _context.NotaFiscal.Remove(notaFiscal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotaFiscalExists(Guid id)
        {
            return _context.NotaFiscal.Any(e => e.Id == id);
        }
    }
}
