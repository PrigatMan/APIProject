using APIProject.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using FluentValidation;

namespace APIProject.Controllers
{
    [ApiController]
    public class ProdusController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IValidator<Produs> _validator;

        public ProdusController(IValidator<Produs> validator, IConfiguration configuration)
        {
            _validator = validator;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        [Route("api/[controller]/GetAll")]
        public IEnumerable<Produs> GetAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<Produs>("SELECT * FROM Produs");
            }
        }

        [HttpPost]
        [Route("api/[controller]/Add")]
        public Result Add(Produs value)
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
                        dbConnection.Execute("INSERT INTO Produs (Id, Denumire, Stoc, Pret) VALUES (@Id, @Denumire, @Stoc, @Pret)", value);
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
        public Result Edit(int id, Produs value)
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
                        dbConnection.Execute("UPDATE Produs SET Denumire = @Denumire, Stoc = @Stoc, Pret = @Pret WHERE Id = @Id", value);
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
                    r.Success = false;
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
                int rowsAffected = dbConnection.Execute("DELETE FROM Produs WHERE Id = @Id", new { Id = id });
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
    }
}
