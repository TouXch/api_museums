using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_museum.Models;
using Microsoft.AspNetCore.Cors;

namespace API_museum.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        public readonly BdMuseumContext _dbContext;

        public ArticleController(BdMuseumContext _context)
        {
            _dbContext = _context;
        }

        //LISTAR TODOS LOS ARTICULOS EXISTENTES
        [HttpGet]
        [Route("list")]
        public IActionResult List()
        {
            List<TbArticle> list = new List<TbArticle>();
            try
            {
                list = _dbContext.TbArticles.Include(m => m.oMuseum).ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        //LISTAR ARTICULOS POR MUSEOS
        [HttpGet]
        [Route("listByMuseum/{idMuseum:int}")]
        public IActionResult listByMuseum(int idMuseum)
        {
            List<TbArticle> articles = new List<TbArticle>();
            List<TbArticle> museumArticles = new List<TbArticle>();

            try
            {
                articles = _dbContext.TbArticles.ToList();
                foreach (var a in articles)
                {
                    if (a.Idmuseum == idMuseum)
                    {
                        museumArticles.Add(a);
                    }
                }
                if (museumArticles.Count == 0)
                {
                    return BadRequest("El museo seleccionado no tiene articulos registrados");
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = museumArticles });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = museumArticles });
            }
        }

        //AGREGAR ARTICULOS
        [HttpPost]
        [Route("addArticle")]
        public IActionResult addArticle([FromBody] TbArticle article)
        {
            try
            {
                _dbContext.TbArticles.Add(article);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Articulo agregado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        //AGREGAR MAS DE 1 ARTICULO
        [HttpPost]
        [Route("addArticles")]
        public IActionResult addArticles([FromBody] List<TbArticle> articles)
        {
            try
            {
                foreach (var a in articles)
                {
                    _dbContext.TbArticles.Add(a);
                }
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Articulos agregados correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        //ELIMINAR ARTICULOS
        [HttpDelete]
        [Route("deleteArticle/{idArticle:int}")]
        public IActionResult deleteArticle(int idArticle)
        {
            TbArticle article = _dbContext.TbArticles.Find(idArticle);

            if (article == null)
            {
                return BadRequest("Articulo no encontrado");
            }

            try
            {
                _dbContext.Remove(article);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Articulo eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        //MARCAR ARTICULOS COMO DANADOS
        [HttpPut]
        [Route("markAsDamaged/{idArticle:int}")]
        public IActionResult markAsDamaged(int idArticle)
        {
            TbArticle oArticle = _dbContext.TbArticles.Find(idArticle);

            if (oArticle == null)
            {
                return BadRequest("No existe el articulo");
            }

            try
            {
                oArticle.Isdamaged = true;
                _dbContext.Update(oArticle);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Articulo marcado como dañado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        //MOVER ARTICULOS DE UN MUSEO A OTRO
        [HttpPut]
        [Route("moveArticle/{idArticle:int}/{idMuseum:int}")]
        public IActionResult moveArticle(int idArticle, int idMuseum)
        {
            TbArticle oArticle = _dbContext.TbArticles.Find(idArticle);

            if (oArticle == null)
            {
                return BadRequest("No existe el articulo");
            }

            try
            {
                oArticle.Idmuseum = idMuseum;
                _dbContext.Update(oArticle);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Articulo movido correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }
    }
}
