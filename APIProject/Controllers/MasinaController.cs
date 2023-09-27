using APIProject.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using FluentValidation;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIProject.Controllers
{
    [ApiController]
    public class MasinaController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IValidator<Masina> _validator;

        public MasinaController(IValidator<Masina> validator, IConfiguration configuration)
        {
            _validator = validator;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // GET: api/<MasinaController>
        [HttpGet]
        [Route("api/[controller]/GetAll")]
        public IEnumerable<Masina> GetAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<Masina>("SELECT * FROM Masina");
            }
        }

        [HttpPost]
        [Route("api/[controller]/Add")]
        public Result Add(Masina value)
        {
            Result r = new Result();
            var validationResult = _validator.Validate(value);
            if (validationResult.IsValid)
            {
                try
                {
                    using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                    {
                        dbConnection.Open();
                        dbConnection.Execute("INSERT INTO Masina (Id, Marca, Model, An, Motor) VALUES (@Id, @Marca, @Model, @An, @Motor)", value);
                    }
                    r.Success = true;
                    r.Message = "Operation successful";
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        r.Errors.Add("The ID is Already present");
                    }
                        
                    return r;
                }
                
            }
            else
            {
                r.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            }
            return r;
        }

        [HttpPut]
        [Route("api/[controller]/Edit/{id}")]
        public Result Edit(int id, Masina value)
        {
            Result r = new Result();
            var validationResult = _validator.Validate(value);
            if (validationResult.IsValid)
            {
                try 
                {
                    using(IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                        dbConnection.Open();
                        dbConnection.Execute("UPDATE Masina SET Marca = @Marca, Model = @Model, An = @An, Motor = @Motor WHERE Id = @Id", value);
                    }
                    r.Success = true;
                    r.Message = "Operation successful";
                }
                catch (SqlException ex) 
                {
                    if (ex.Number == 2627)
                    {
                        r.Errors.Add("The ID is Already present");
                    }
                    return r;
                }            
            }
            else
            {
                r.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            }
            return r;
        }

        [HttpDelete]
        [Route("api/[controller]/Delete/{id}")]
        public Result Delete(int id)
        {
            Result r = new Result();
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                int rowsAffected = dbConnection.Execute("DELETE FROM Masina WHERE Id = @Id", new { Id = id });
                if (rowsAffected > 0)
                {
                    r.Success = true;
                    r.Message = "Operation successful";
                }
                else
                {
                    r.Errors.Add("Id not found");
                }
            }
            return r;
        }
        /*
        private bool Check(Masina value, Result result)
        {
            if (!(value.Id >= 0))
            {
                result.Message = "The Id value can't be negative";
                return false;
            }
            if (string.IsNullOrWhiteSpace(value.Marca))
            {
                result.Message = "You must insert a car manufacturer";
                return false;
            }
            if (string.IsNullOrWhiteSpace(value.Model))
            {
                result.Message = "You must insert the car model";
                return false;
            }
            if (!(value.An >= 1889))
            {
                result.Message = "The year value must be higher than 1899";
                return false;
            }
            if (!(value.Motor > 0))
            {
                result.Message = "The engine size value must be higher than 0";
                return false;
            }
            return true;
        }*/
    }
}

