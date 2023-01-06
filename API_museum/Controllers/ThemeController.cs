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
    public class ThemeController : ControllerBase
    {
        public readonly BdMuseumContext _dbcontext;

        public ThemeController(BdMuseumContext _context)
        {
            _dbcontext = _context;
        }

        //LISTAR TODAS LAS TEMATICAS
        [HttpGet]
        [Route("list")]
        public IActionResult List()
        {
            List<TbTheme> themeList = new List<TbTheme>();
            try
            {
                themeList = _dbcontext.TbThemes.ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = themeList });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = themeList });
            }
        }

        //OBTENER UNA TEMATICA POR EL ID
        [HttpGet]
        [Route("getByid/{idtheme:int}")]
        public IActionResult getById(int idtheme)
        {
            TbTheme theme = new TbTheme();
            try
            {
                theme = _dbcontext.TbThemes.Find(idtheme);
                return StatusCode(StatusCodes.Status200OK, new {message="ok", response = theme});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        //AGREGAR UNA TEMATICA
        [HttpPost]
        [Route("saveTheme")]
        public IActionResult Save([FromBody] TbTheme theme)
        {
            try
            {
                _dbcontext.TbThemes.Add(theme);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Tematica creada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        //EDITAR UNA TEMATICA
        [HttpPut]
        [Route("editTheme")]
        public IActionResult Edit([FromBody] TbTheme theme)
        {
            TbTheme oTheme = _dbcontext.TbThemes.Find(theme.Idtheme);

            if (oTheme == null)
            {
                return BadRequest("La tematica seleccionada no existe");
            }

            try
            {
                oTheme.Name = theme.Name is null ? oTheme.Name : theme.Name;

                _dbcontext.Update(oTheme);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "Tematica modficada con exito" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        //ELIMINAR UNA TEMATICA
        [HttpDelete]
        [Route("delete/{idTheme:int}")]
        public IActionResult Delete(int idTheme)
        {
            TbTheme oTheme = _dbcontext.TbThemes.Find(idTheme);

            if (oTheme == null)
            {
                return BadRequest("Tematica no encontrada");
            }

            try
            {
                _dbcontext.Remove(oTheme);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Tematica eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }
    }
}
