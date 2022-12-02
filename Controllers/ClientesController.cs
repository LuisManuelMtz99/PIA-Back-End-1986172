using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIA___Back___End___1986172.DTOs;
using PIA___Back___End___1986172.Entidades;

namespace PIA___Back___End___1986172.Controllers
{
    [ApiController]
    [Route("clientes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<ClientesController> logger;
        private readonly UserManager<IdentityUser> userManager;

        public ClientesController(ApplicationDbContext context, IMapper mapper, ILogger<ClientesController> logger, UserManager<IdentityUser> userManager)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult<List<GetClienteDTO>>> Get()
        {
            var clientes = await dbContext.Clientes.ToListAsync();
            return mapper.Map<List<GetClienteDTO>>(clientes);
        }

        [HttpGet("{numeroCliente:int}", Name = "obtenerCliente")]
        public async Task<ActionResult<GetClienteDTO>> Get(int numeroCliente)
        {
            var cliente = await dbContext.Clientes.FirstOrDefaultAsync(x => x.NumeroCliente == numeroCliente);

            if (cliente == null)
            {
                return NotFound();
            }

            return mapper.Map<GetClienteDTO>(cliente);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Post(ClienteCreacionDTO clienteCreacionDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;
            var existeCliente = await dbContext.Clientes.AnyAsync(x => x.NumeroCliente == clienteCreacionDTO.NumeroCliente);

            if (existeCliente)
            {
                return BadRequest($"Ya existe un cliente con el numero: {clienteCreacionDTO.NumeroCliente}");
            }

            var cantidadDeClientes = await dbContext.Clientes.CountAsync();
            if (cantidadDeClientes >= 54)
            {
                return BadRequest($"Ya se alcanzo el limite de clientes participantes (54 participantes)");
            }

            var cliente = mapper.Map<Cliente>(clienteCreacionDTO);
            cliente.UsuarioId = usuarioId;
            dbContext.Add(cliente);
            await dbContext.SaveChangesAsync();
            var clienteDTO = mapper.Map<GetClienteDTO>(cliente);
            return CreatedAtRoute("obtenerCliente", new { numeroCliente = cliente.NumeroCliente }, clienteDTO);
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Put(ClienteCreacionDTO clienteCreacionDTO, int id)
        {

            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var exist = await dbContext.Clientes.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var cliente = mapper.Map<Cliente>(clienteCreacionDTO);
            cliente.Id = id;
            cliente.UsuarioId = usuarioId;

            if (cliente.Id != id)
            {
                return BadRequest("El id del cliente no coincide con el establecido en la url.");
            }

            dbContext.Update(cliente);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

       
        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
       
            var existeCliente = await dbContext.Clientes.AnyAsync(x => x.Id == id);
            if (!existeCliente)
            {
                return NotFound("El cliente no fue encontrado.");
            }

            dbContext.Remove(new Cliente()
            {
                Id = id
            });

            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
