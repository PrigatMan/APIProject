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
    public class PersoanaController : ControllerBase
    {
        private readonly string _connectionString;

        public PersoanaController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: api/<PersoanaController>
        [HttpGet]
        [Route("api/[controller]/GetAll")]
        public IEnumerable<Persoana> GetAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<Persoana>("SELECT * FROM Persoana");
            }
        }

        // POST api/<PersoanaController>
        [HttpPost]
        [Route("api/[controller]/Add")]
        public Result Add(Persoana value)
        {
            Result r = new Result();
            if (Check(value, r))
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    var ValidId = dbConnection.Query<Persoana>("SELECT * FROM Persoana WHERE Id = @Id", new { value.Id }).ToList();
                    if (!ValidId.Any())
                    {
                        r.Success = true;
                        r.Message = "Operation successful";
                        dbConnection.Execute("INSERT INTO Persoana (Id, Nume, Prenume, Adresa, Email) VALUES (@Id, @Nume, @Prenume, @Adresa, @Email)", value);
                    }
                    else
                    {
                        r.Message = "The inserted id is already present";
                    }
                }
            }
            else
            {
                r.Message = "One of the inserted values is not accepted";
            }

            return r;
        }

        // PUT api/<PersoanaController>/5
        [HttpPut]
        [Route("api/[controller]/Edit/{id}")]
        public Result Edit(int id, Persoana value)
        {
            Result r = new Result();
            if (Check(value, r, id))
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    var ValidId = dbConnection.Query<Persoana>("SELECT * FROM Persoana WHERE Id = @Id", new { id }).ToList();
                    if (ValidId.Count == 1 && ValidId.First().Id == id || !ValidId.Any())
                    {
                        r.Success = true;
                        r.Message = "Operation successful";
                        dbConnection.Execute("UPDATE Persoana SET Nume = @Nume, Prenume = @Prenume, Adresa = @Adresa, Email = @Email WHERE Id = @Id", value);
                    }
                    else
                    {
                        r.Message = "The inserted id is already present";
                    }
                }
            }
            else
            {
                r.Message = "One of the inserted values is not accepted";
            }

            return r;
        }

        // DELETE api/<PersoanaController>/5
        [HttpDelete]
        [Route("api/[controller]/Delete/{id}")]
        public Result Delete(int id)
        {
            Result r = new Result();
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                int affectedRows = dbConnection.Execute("DELETE FROM Persoana WHERE Id = @Id", new { id });
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

        private bool Check(Persoana value, Result result)
        {
            if (value.Id < 0)
            {
                result.Message = "The Id value can't be negative";
                return false;
            }
            if (string.IsNullOrWhiteSpace(value.Nume))
            {
                result.Message = "You must insert a name for the person";
                return false;
            }
            if (string.IsNullOrWhiteSpace(value.Prenume))
            {
                result.Message = "You must insert a surname for the person";
                return false;
            }
            if (string.IsNullOrWhiteSpace(value.Adresa))
            {
                result.Message = "You must insert an address for the person";
                return false;
            }
            if (string.IsNullOrWhiteSpace(value.Email))
            {
                result.Message = "You must insert an email for the person";
                return false;
            }
            return true;
        }

        private bool Check(Persoana value, Result result, int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var ValidId = dbConnection.Query<Persoana>("SELECT * FROM Persoana WHERE Id = @Id", new { id }).ToList();
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
                if (string.IsNullOrWhiteSpace(value.Nume))
                {
                    result.Message = "You must insert a name for the person";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(value.Prenume))
                {
                    result.Message = "You must insert a surname for the person";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(value.Adresa))
                {
                    result.Message = "You must insert an address for the person";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(value.Email))
                {
                    result.Message = "You must insert an email for the person";
                    return false;
                }
                return true;
            }
        }
    }
}
