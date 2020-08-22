﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackEndAD.TempService;
using BackEndAD.Models;
using BackEndAD.ServiceInterface;
using System;
using System.Linq;

//REMINDER: All existing comments generated by BiancaZYCao
//This is an simple example about how to code Web API controller return data result for ReactJS
//
namespace BackEndAD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {

        private IStoreClerkService _clkService;
        
        private IStoreManagerService _mgrService;
        public StoreController(IStoreClerkService clkService, IStoreManagerService mgrService)
        {
            _clkService = clkService;
            _mgrService = mgrService;
        }

        #region Stationery List (Inventory)
        [HttpGet("Stationeries")]
        public async Task<ActionResult<List<Stationery>>> GetAllStationeries()
        {
            var result = await _clkService.findAllStationeriesAsync();
            // if find data then return result else will return a String says Department not found
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                return NotFound("Stationeries not found");
        }

        //Post Request for stationery by id
        [HttpPost("Stationery/post")]
        public Task<ActionResult<Stationery>> PostStationery([FromBody] Stationery stationery)
        {
            Console.WriteLine("stationaryPost");
            Stationery s1 = new Stationery()
            {
                Id = stationery.Id,
                category = stationery.category,
                desc = stationery.desc,
                inventoryQty = stationery.inventoryQty
            };
            //_clkService.saveStationery(s1);
            return null;
        }

        [HttpGet("Stationeries/{id}")]
        public async Task<ActionResult<Stationery>> GetStationeryByIdAsync(int id)
        {
            var result = await _clkService.findStationeryByIdAsync(id);
            // if find data then return result else will return a String says Department not found
            if (result != null)
                return Ok(result);
            else
                return NotFound("Stationery not found");
        }
        #endregion

        [HttpGet("Suppliers")]
        public async Task<ActionResult<List<Supplier>>> GetAllSuppliers()
        {
            var result = await _clkService.findAllSuppliersAsync();
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                return NotFound("Suppliers not found");
        }

        [HttpPost("saveSupplier")]
        public async Task<ActionResult<Supplier>> saveSupplier([FromBody] Supplier s)
        {
            Supplier sup = new Supplier()
            {
                supplierCode = s.supplierCode,
                name = s.name,
                contactPerson = s.contactPerson,
                email = s.email,
                phoneNum = s.phoneNum,
                gstRegisNo = s.gstRegisNo,
                fax = s.fax,
                address = s.address,
                priority = s.priority,
            };
            _clkService.saveSupplier(sup);

            return CreatedAtAction(nameof(GetAllSuppliers), new { }, sup);
        }

        [HttpPost("deleteSupplier")]
        public async Task<ActionResult<Supplier>> DeleteSupplier([FromBody] Supplier s)
        {
            _clkService.deleteSupplier(s.Id);
            return CreatedAtAction(nameof(GetAllSuppliers), new { }, s);
        }

        [HttpPost("updateSupplier")]
        public async Task<ActionResult<Supplier>> UpdateSupplier([FromBody] Supplier sup)
        {

            _clkService.updateSupplier(sup);
            return CreatedAtAction(nameof(GetAllSuppliers), new { }, sup);

        }

        //StoreManager stockadjustment voucher
        [HttpGet("adjustmentList")]
        public async Task<ActionResult<List<StockAdjustSumById>>> GetAllAdustmentInfo()
        {
            
            var result = await _mgrService.StockAdjustDetailInfo();
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                return NotFound("Error");
        }

        
        [HttpPost("getAllAdjustDetailLine")]
        public async Task<ActionResult<List<AdjustmentVocherInfo>>> getAllAdjustDetailLine([FromBody] StockAdjustSumById item)
        {
            var result = await _mgrService.getAllAdjustDetailLineByAdjustId(item);
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                return NotFound("Error");
        }

        [HttpPost("issueVoucher")]
        public async Task<ActionResult<List<AdjustmentVocherInfo>>> CreateVoucher([FromBody] StockAdjustSumById voc)
        {
            var result = await _mgrService.issueVoucher(voc);
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                return NotFound("Error");

        }

        [HttpPost("getVoucher")]
        public async Task<ActionResult<AdjustmentVocherInfo>> getVoucher([FromBody] AdjustmentVocherInfo voc)
        {
            var result = await _mgrService.getEachVoucherDetail(voc);
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                return NotFound("Error");
        }
        
        //end

        #region Test post method 18Aug
        [HttpPost("stkAd")]
        public /*async*/ Task<ActionResult<StockAdjustment>> PostTestStkAd(
               [FromBody] List<StockAdjustmentDetail> stockAdjustmentDetails)
        {
            Console.WriteLine("post");
            //Console.WriteLine(id);
            Console.WriteLine(stockAdjustmentDetails[0].comment);
            /*StockAdjustment stkAdj = new StockAdjustment()
            {
                date = DateTime.Now,
                type = "inventory check",
                EmployeeId = id
            };

            var result = await _clkService.generateStkAdjustmentAsync(stkAdj, stockAdjustmentDetails); //SaveChangesAsync();
            if (result != null)
                return CreatedAtAction(
                    nameof(GetStkAdjId), new { id = result.Id }, result);
            else
                return NotFound("Sry failed.");*/
            return null;
        }

        [HttpGet("stkAd/get/{id}")]
        public async Task<ActionResult<StockAdjustment>> GetStkAdjId(int id)
        {
            var result = await _clkService.findStockAdjustmentByIdAsync(id);
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                return NotFound("Suppliers not found");
        }
        #endregion

        #region place order 
        [HttpGet("placeOrder")]
        public async Task<ActionResult<ReOrderRecViewModel>> GetReOrderRec()
        {
            //iterate through stationery
            var stationeries = await _clkService.findAllStationeriesAsync();
            IList<ReOrderRecViewModel> result = new List<ReOrderRecViewModel>();
            int id = 0;

            foreach (Stationery s in stationeries)
            {
                //find all suppliers that supply stationery 
                ICollection<Supplier> suppliers = await _clkService.findSupplierByStationeryId(1);
                //s.Id);

                //create a ReOrderRecViewModel for each stationery
                ReOrderRecViewModel reorder = new ReOrderRecViewModel()
                {
                    id = id,
                    stationery = s,
                    suppliers = suppliers
                };

                result.Add(reorder);
                id++;
            }

            if (result != null)
            {
                return Ok(result);
            }

            else
                return NotFound("No reorder items at this time.");
        }

        //api to get current clerk Id [HttpGet("/clerk")]

        [HttpPost("/generatePO")]
        public Task<ActionResult<PurchaseOrder>> PostPurchaseOrder(List<PurchaseOrder> purchaseOrders)
        {
            for (int i = 0; i < purchaseOrders.Count; i++)
            {
                PurchaseOrder po = purchaseOrders[i];
                _clkService.savePurchaseOrder(po);
            }
            return null;
        }
        [HttpGet("ItemsNeedOrder")]
        public async Task<ActionResult<List<Stationery>>> GetItemsNeedOrder()
        {
            //iterate through stationery
            IList<Stationery> stationeries = await _clkService.findAllStationeriesAsync();
            List<Stationery> itemsNeedOrder =
                stationeries.Where(x => x.inventoryQty < x.reOrderLevel).ToList();
            return itemsNeedOrder;
            
        }
        [HttpGet("getSupplierItems/{id}")]
        public IList<SupplierItem> GetSupplierItemsListByStationeryId(int id)
        {
            IList<SupplierItem> result = _clkService.findSuppliersByStationeryId(id);
            foreach (SupplierItem s in result)
            {
                Console.WriteLine(s.StationeryId);
                    }
            if (result != null)
            {
                return result;
            }
            else
                return null;
            

        }

        #endregion


    }
}