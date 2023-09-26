using APIProject.Model;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace APIProject.Controllers
{
    [ApiController]
    public class ProdusController : ControllerBase
    {
        private readonly string _connectionString;

        public ProdusController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        List<Produs> produs = new List<Produs>();

        public ProdusController()
        {
            produs.Add(new Produs { Id = 0, Denumire = "Rosi", Stoc = 344, Pret = 4.99 });
            // Add other initial products here...
        }

        // GET: api/<ProdusController>
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

        // POST api/<ProdusController>
        [HttpPost]
        [Route("api/[controller]/Add")]
        public Result Add(Produs value)
        {
            Result r = new Result();
            if (Check(value, r))
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    var ValidId = dbConnection.Query<Produs>("SELECT * FROM Produs WHERE Id = @Id", new { value.Id }).ToList();
                    if (!ValidId.Any())
                    {
                        r.Success = true;
                        r.Message = "Operation successful";
                        dbConnection.Execute("INSERT INTO Produs (Id, Denumire, Stoc, Pret) VALUES (@Id, @Denumire, @Stoc, @Pret)", value);
                    }
                    else
                    {
                        r.Message = "The inserted id is already present";
                    }
                }
            }
            return r;
        }

        // PUT api/<ProdusController>/5
        [HttpPut]
        [Route("api/[controller]/Edit/{id}")]
        public Result Edit(int id, Produs value)
        {
            Result r = new Result();
            if (Check(value, r, id))
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    var ValidId = dbConnection.Query<Produs>("SELECT * FROM Produs WHERE Id = @Id", new { id }).ToList();
                    if (ValidId.Count == 1 && ValidId.First().Id == id || !ValidId.Any())
                    {
                        r.Success = true;
                        r.Message = "Operation successful";
                        dbConnection.Execute("UPDATE Produs SET Denumire = @Denumire, Stoc = @Stoc, Pret = @Pret WHERE Id = @Id", value);
                    }
                    else
                    {
                        r.Message = "The inserted id is already present";
                    }
                }
            }
            return r;
        }

        // DELETE api/<ProdusController>/5
        [HttpDelete]
        [Route("api/[controller]/Delete/{id}")]
        public Result Delete(int id)
        {
            Result r = new Result();
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                int affectedRows = dbConnection.Execute("DELETE FROM Produs WHERE Id = @Id", new { id });
                if (affectedRows > 0)
                {
                    r.Success = true;
                    r.Message = "Operation successful";
                }
                else
                {
                    r.Message = "Id not found";
                }
            }
            return r;
        }

        private bool Check(Produs value, Result result)
        {
            if (value.Id < 0)
            {
                result.Message = "The Id value can't be negative";
                return false;
            }
            if (string.IsNullOrWhiteSpace(value.Denumire))
            {
                result.Message = "You must insert a name for the product";
                return false;
            }
            if (value.Stoc < 0)
            {
                result.Message = "The Stoc value can't be negative";
                return false;
            }
            if (value.Pret <= 0)
            {
                result.Message = "The Pret value must be higher than 0";
                return false;
            }
            return true;
        }

        private bool Check(Produs value, Result result, int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var ValidId = dbConnection.Query<Produs>("SELECT * FROM Produs WHERE Id = @Id", new { id }).ToList();
                if (!ValidId.Any())
                {
                    result.Message = "Id not found";
                    return false;
                }
                if (value.Id < 0)
                {
                    result.Message = "The Id value can't be negative";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(value.Denumire))
                {
                    result.Message = "You must insert a name for the product";
                    return false;
                }
                if (value.Stoc < 0)
                {
                    result.Message = "The Stoc value can't be negative";
                    return false;
                }
                if (value.Pret <= 0)
                {
                    result.Message = "The Pret value must be higher than 0";
                    return false;
                }
                return true;
            }
        }
    }
}
