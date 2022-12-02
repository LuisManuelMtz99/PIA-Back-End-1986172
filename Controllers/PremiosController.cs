using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIA___Back___End___1986172.DTOs;
using PIA___Back___End___1986172.Entidades;

namespace PIA___Back___End___1986172.Controllers
{
    [ApiController]
    [Route("premios")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PremiosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<ClientesController> logger;

        public PremiosController(ApplicationDbContext context, IMapper mapper, ILogger<ClientesController> logger)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetPremioDTO>>> Get()
        {
            var premio = await dbContext.Premios.ToListAsync();
            return mapper.Map<List<GetPremioDTO>>(premio);
        }

        [HttpGet("premioDeRifa/{numeroRifa:int}", Name = "ObtenerPremio")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult<List<Rifa>>> Get(int numeroRifa)
        {
            var rifa = await dbContext.Rifas.FirstOrDefaultAsync(x => x.NumeroRifa == numeroRifa);
            if (rifa == null)
            {
                return NotFound();
            }

            var existePremio = await dbContext.Premios.AnyAsync(premioDB => premioDB.RifaId == rifa.Id);
            if (!existePremio)
            {
                return NotFound();
            }
            int premioRifaCount = await dbContext.Premios.CountAsync(premioDB => premioDB.RifaId == rifa.Id).ConfigureAwait(false);
            logger.LogInformation(premioRifaCount.ToString());
            Random r = new Random();
            int numeroRandom = r.Next(0, premioRifaCount);
            var premio = await dbContext.Premios.Skip(numeroRandom).FirstOrDefaultAsync(premioDB => premioDB.RifaId == rifa.Id);
            return Ok(premio);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Post(PremioCreacionDTO premioCreacionDTO)
        {
            var existePremio = await dbContext.Premios.AnyAsync(x => x.NombrePremio == premioCreacionDTO.NombrePremio);

            if (existePremio)
            {
                return BadRequest($"Ya existe el premio: {premioCreacionDTO.NombrePremio}");
            }

            var premio = mapper.Map<Premio>(premioCreacionDTO);
            dbContext.Add(premio);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Put(PremioCreacionDTO premioCreacionDTO, int id)
        {
            var exist = await dbContext.Premios.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var premio = mapper.Map<Premio>(premioCreacionDTO);
            premio.Id = id;

            if (premio.Id != id)
            {
                return BadRequest("El id del premio no coincide con el establecido en la url.");
            }

            dbContext.Update(premio);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

       

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Premios.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El premio no fue encontrado.");
            }

            dbContext.Remove(new Premio()
            {
                Id = id
            });

            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}