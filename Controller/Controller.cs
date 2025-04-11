using Microsoft.AspNetCore.Mvc;
using MedicineStoreAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedicineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private static List<Medicine> medicines = new List<Medicine>
        {
            new Medicine { MedicineId = 1, Name = "Paracetamol", Description = "Used for fever and pain relief", Price = 20, Stock = 50 },
            new Medicine { MedicineId = 2, Name = "Ibuprofen", Description = "Used for inflammation and pain relief", Price = 50, Stock = 30 },
            new Medicine { MedicineId = 3, Name = "Amoxicillin", Description = "Antibiotic for infections", Price = 100, Stock = 20 }
        };

        // ✅ Get all medicines
        [HttpGet]
        public ActionResult<IEnumerable<Medicine>> GetMedicines()
        {
            return Ok(medicines);
        }

        // ✅ Get medicine by ID
        [HttpGet("{id}")]
        public ActionResult<Medicine> GetMedicine(int id)
        {
            var medicine = medicines.FirstOrDefault(m => m.MedicineId == id);
            if (medicine == null)
            {
                return NotFound(new { message = "Medicine not found!" });
            }
            return Ok(medicine);
        }

        // ✅ Add a new medicine
        [HttpPost]
        public ActionResult AddMedicine([FromBody] Medicine newMedicine)
        {
            if (newMedicine == null)
            {
                return BadRequest(new { message = "Invalid medicine data!" });
            }

            // Get the highest existing MedicineId and increment it
            int newId = medicines.Any() ? medicines.Max(m => m.MedicineId) + 1 : 1;
            newMedicine.MedicineId = newId;

            medicines.Add(newMedicine);

            return CreatedAtAction(nameof(GetMedicine), new { id = newMedicine.MedicineId }, newMedicine);
        }

        // ✅ Delete a medicine
        [HttpDelete("{id}")]
        public ActionResult DeleteMedicine(int id)
        {
            var medicine = medicines.FirstOrDefault(m => m.MedicineId == id);
            if (medicine == null)
            {
                return NotFound(new { message = "Medicine not found!" });
            }

            medicines.Remove(medicine);
            return Ok(new { message = "Medicine deleted successfully!", remainingMedicines = medicines });
        }
    }
}
