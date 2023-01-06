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
    public class MuseumController : ControllerBase
    {
        public readonly BdMuseumContext _dbcontext;

        public MuseumController(BdMuseumContext _context)
        {
            _dbcontext = _context;
        }

        //LISTAR TODOS LOS MUSEOS
        [HttpGet]
        [Route("list")]
        public IActionResult list()
        {
            List<TbMuseum> museums = new List<TbMuseum>();
            try
            {
                museums = _dbcontext.TbMuseums.ToList();
                return StatusCode(StatusCodes.Status200OK, new {message= "ok", response=museums});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = museums });
            }
        }

        //OBTENER MUSEO POR EL ID
        [HttpGet]
        [Route("getByid/{idmuseum:int}")]
        public IActionResult getById(int idmuseum)
        {
            TbMuseum museum = new TbMuseum();
            try
            {
                museum = _dbcontext.TbMuseums.Find(idmuseum);
                return StatusCode(StatusCodes.Status200OK, new {message="ok", response=museum});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        //Mostrar todos los museos que pertenecen a la misma tematica
        [HttpGet]
        [Route("listByTheme/{idtheme:int}")]
        public IActionResult ListByTheme(int idtheme)
        {
            List<TbMuseum> museums = new List<TbMuseum>();
            List<TbMuseum> museumsByTheme = new List<TbMuseum>();
            try
            {
                museums = _dbcontext.TbMuseums.ToList();
                foreach (var m in museums)
                {
                    if (m.Idtheme == idtheme)
                    {
                        museumsByTheme.Add(m);
                    }
                }
                if (museumsByTheme.Count == 0)
                {
                    return BadRequest("No hay museos de esa tematica registrados");
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = museumsByTheme });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = museumsByTheme });
            }
        }

        //AGREGAR UN MUSEO
        [HttpPost]
        [Route("saveMuseum")]
        public IActionResult Save([FromBody] TbMuseum museum)
        {
            try
            {
                _dbcontext.TbMuseums.Add(museum);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Museo creado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        //EDITAR UN MUSEO
        [HttpPut]
        [Route("edit")]
        public IActionResult Edit([FromBody] TbMuseum museum)
        {
            TbMuseum oMuseum = _dbcontext.TbMuseums.Find(museum.Idmuseum);

            if (oMuseum == null)
            {
                return BadRequest("El museo seleccionado no existe");
            }

            try
            {
                oMuseum.Name = museum.Name is null ? oMuseum.Name : museum.Name;
                oMuseum.Idtheme = museum.Idtheme is null ? oMuseum.Idtheme : museum.Idtheme;

                _dbcontext.Update(oMuseum);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "Museo modficado con exito" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        //ELIMINAR UN MUSEO
        [HttpDelete]
        [Route("delete/{idMuseum:int}")]
        public IActionResult Delete(int idMuseum)
        {
            TbMuseum oMuseum = _dbcontext.TbMuseums.Find(idMuseum);

            if (oMuseum == null)
            {
                return BadRequest("Museo no encontrado");
            }

            try
            {
                _dbcontext.Remove(oMuseum);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Museo eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }
    }
}
