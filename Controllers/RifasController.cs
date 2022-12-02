using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIA___Back___End___1986172.DTOs;
using PIA___Back___End___1986172.Entidades;

namespace PIA___Back___End___1986172.Controllers
{
    [ApiController]
    [Route("rifas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RifasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<ClientesController> logger;

        public RifasController(ApplicationDbContext context, IMapper mapper, ILogger<ClientesController> logger)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetRifaDTO>>> Get()
        {
         
            var rifa = await dbContext.Rifas.ToListAsync();
            return mapper.Map<List<GetRifaDTO>>(rifa);
        }

        [HttpGet("obtenerBoletoGanador/{numeroRifa:int}", Name = "ObtenerBoletoGanador")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult<List<Rifa>>> Get(int numeroRifa)
        {
            var rifa = await dbContext.Rifas.FirstOrDefaultAsync(x => x.NumeroRifa == numeroRifa);
            if (rifa == null)
            {
                return NotFound();
            }

            var existeBoleto = await dbContext.Boletos.AnyAsync(boletoDB => boletoDB.RifaID == rifa.Id);
            if (!existeBoleto)
            {
                return NotFound();
            }

            int boletosRifaCount = await dbContext.Boletos.CountAsync(boletoDB => boletoDB.RifaID == rifa.Id).ConfigureAwait(false);
            Random r = new Random();
            int numeroRandom = r.Next(0, boletosRifaCount);
            var boletoGanador = await dbContext.Boletos.Skip(numeroRandom).FirstOrDefaultAsync(boletoDB => boletoDB.RifaID == rifa.Id);
            return Ok(boletoGanador);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Post(RifaCreacionDTO rifaCreacionDTO)
        {
            var existeRifa = await dbContext.Rifas.AnyAsync(x => x.NumeroRifa == rifaCreacionDTO.NumeroRifa);

            if (existeRifa)
            {
                return BadRequest($"Ya existe una rifa con este numero");
            }


            var rifa = mapper.Map<Rifa>(rifaCreacionDTO);
            dbContext.Add(rifa);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Put(RifaCreacionDTO rifaCreacionDTO, int id)
        {
            var exist = await dbContext.Rifas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var rifa = mapper.Map<Rifa>(rifaCreacionDTO);
            rifa.Id = id;

            if (rifa.Id != id)
            {
                return BadRequest("El id del cliente no coincide con el establecido en la url.");
            }

            dbContext.Update(rifa);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

       

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
            var existeRifa = await dbContext.Rifas.AnyAsync(x => x.Id == id);
            if (!existeRifa)
            {
                return NotFound("La rifa no fue encontrada.");
            }

            dbContext.Remove(new Rifa()
            {
                Id = id
            });

            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}