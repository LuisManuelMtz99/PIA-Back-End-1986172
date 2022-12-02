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
    [Route("Boletos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BoletosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<ClientesController> logger;

        public BoletosController(ApplicationDbContext context, IMapper mapper, ILogger<ClientesController> logger)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult<List<GetBoletoDTO>>> Get()
        {
            var boleto = await dbContext.Boletos.ToListAsync();
            return mapper.Map<List<GetBoletoDTO>>(boleto);
        }

        [HttpGet("obtenerClienteGanador/{numeroBoleto:int}", Name = "ObtenerClienteGanador")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult<GetClienteDTO>> Get(int numeroBoleto)
        {
            var boleto = await dbContext.Boletos.FirstOrDefaultAsync(x => x.NumeroBoleto == numeroBoleto);
            if (boleto == null)
            {
                return NotFound("");
            }

            var clienteGanador = await dbContext.Clientes.FirstOrDefaultAsync(clienteDB => clienteDB.Id == boleto.ClienteID);
            if (clienteGanador == null)
            {

                return NotFound();
            }

            return mapper.Map<GetClienteDTO>(clienteGanador);
        }

        [HttpPost]
        public async Task<ActionResult> Post(BoletoCreacionDTO boletoCreacionDTO)
        {
            var existeBoleto = await dbContext.Boletos.AnyAsync(x => x.NumeroBoleto == boletoCreacionDTO.NumeroBoleto);

            if (existeBoleto)
            {
                return BadRequest($"Ya existe un boleto con el numero: {boletoCreacionDTO.NumeroBoleto}");
            }

            var boleto = mapper.Map<Boleto>(boletoCreacionDTO);
            dbContext.Add(boleto);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Put(BoletoCreacionDTO boletoCreacionDTO, int id)
        {
            var exist = await dbContext.Boletos.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var boleto = mapper.Map<Boleto>(boletoCreacionDTO);
            boleto.Id = id;

            if (boleto.Id != id)
            {
                return BadRequest("El id del cliente no coincide con el establecido en la url.");
            }

            dbContext.Update(boleto);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Delete(int id)
        {

            var exist = await dbContext.Boletos.AnyAsync(x => x.Id == id);
            if (!exist)
            {

                return NotFound("El boleto no fue encontrado.");
            }

            dbContext.Remove(new Boleto()
            {
                Id = id
            });

            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}