﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackEndAD.Models;
using BackEndAD.ServiceInterface;
using System.Text.RegularExpressions;
using BackEndAD.TempService;

//REMINDER: All existing comments generated by BiancaZYCao
//This is an simple example about how to code Web API controller return data result for ReactJS
//
namespace BackEndAD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        //GET DATA result from SERVICE layer 
        //e.g. here IEmployeeService is an interface do all the stuff related to ENTITY- emp
        //Here we should call fewer service to make code reusable and clean 
        //private IEmployeeService _empService; not used so far 
        private IDepartmentService _deptService;
        private IStoreClerkService _clerkService;

        //CONSTRUCTOR: make sure u build ur service interface in.
        public DeptController(IDepartmentService deptService, IStoreClerkService clerkService)
        {
            _deptService = deptService;
            _clerkService = clerkService;
        }

        // CONTROLLER METHODS handling each HTTP get/put/post/request
        #region basic info-DEPT+EMP+CollectionPoint with eager loading example
        // GET: api/Dept
        [HttpGet]
        public async Task<ActionResult<IList<Department>>> GetAllDepartments()
        {
            var result = await _deptService.findAllDepartmentsAsync();
            // if find data then return result else will return a String says Department not found
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                //There are 3 Department alr seeded in DB, so this line should nvr appears. 
                //I put here Just for u to understand the style. :) -Bianca  
                return NotFound("Departments not found.");
        }

        //return dept info by id
        // GET: api/Dept/id (data passing via URL)
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartmentByIdAsync(int id)
        {
            var resultDepartment = await _deptService.findDepartmentByIdAsync(id);
            // if find data then return result else will return a String says Department not found
            if (resultDepartment != null)
                return Ok(resultDepartment);
            else
                return NotFound("Department not found.");
        }

        [HttpGet("allCollectionpt")]
        public async Task<ActionResult<IList<CollectionInfo>>> GetAllCollectionPointforDept()
        {
            var allCollectionPtList = await _deptService.findAllCollectionPointAsync();

            // if find data then return result else will return a String says Department not found
            if (allCollectionPtList != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(allCollectionPtList);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                //There are 3 Department alr seeded in DB, so this line should nvr appears. 
                //I put here Just for u to understand the style. :) -Bianca  
                return NotFound("No collection point found");
        }

        //This Eager Loading can fetch data but cannot transfer into Json properly -Bianca
        [HttpGet("eager")]
        public ActionResult<List<CollectionInfo>> GetAllCollectionInfoEager()
        {
            var resultL = _deptService.findAllDepartmentsAsyncEager();
            var result = new List<CollectionInfo>(){ };
            
            //var result = new List<Int64>() { };
            foreach (Department dept in resultL)
            {
                Console.WriteLine(dept.Collection.lat);
                result.Add(dept.Collection);
             }
            if (result != null)
            {
                var result2 = result.First<CollectionInfo>();
                return Ok(result2);//.First<Department>().Collection.Id);
            }
            else
                return NotFound("Eager No way!");
        }

        [HttpGet("emp")]
        public async Task<ActionResult<IList<Employee>>> GetAllEmployees()
        {
            var allEmployeesList = await _deptService.findAllEmployeesAsync();

            // if find data then return result else will return a String says Department not found
            if (allEmployeesList != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(allEmployeesList);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                //There are 3 Department alr seeded in DB, so this line should nvr appears. 
                //I put here Just for u to understand the style. :) -Bianca  
                return NotFound("Employees not found.");
        }
        #endregion

        #region requisition
        [HttpGet("req")]
        public async Task<ActionResult<IList<Requisition>>> GetAllRequisitions()
        {
            var result = await _deptService.findAllRequsitionsAsync();
            //var result2 = result.First<Requisition>().Employee;
            // if find data then return result else will return a String says Department not found
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                //There are 3 Department alr seeded in DB, so this line should nvr appears. 
                //I put here Just for u to understand the style. :) -Bianca  
                return NotFound("Requisition not found.");
        }
        #endregion

        #region requisition details
        [HttpGet("reqDetails")]
        public async Task<ActionResult<IList<Requisition>>> GetAllRequisitionsDetails()
        {
            var result = await _deptService.findAllRequsitionDetailAsync();
            //var result2 = result.First<Requisition>().Employee;
            // if find data then return result else will return a String says Department not found
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                //There are 3 Department alr seeded in DB, so this line should nvr appears. 
                //I put here Just for u to understand the style. :) -Bianca  
                return NotFound("Requisition Details not found.");
        }

        [HttpPost("getAllItemList")]
        public async Task<ActionResult<List<Requisition>>> getAllItemList([FromBody] Requisition req)
        {
            var result = await _deptService.findAllRequisitionDetailsItemListById(req);
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                return NotFound("Error");
        }
        #endregion

        #region requisition apply
        [HttpPost("ApplyRequisition")]
        public async Task<ActionResult<IList<RequisitionDetail>>> ApplyRequisition([FromBody] List<RequisitionDetailsApply>requisition)
        {
            var result = await _deptService.applyRequisition(requisition);
            // if find data then return result else will return a String says Department not found
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                //There are 3 Department alr seeded in DB, so this line should nvr appears. 
                //I put here Just for u to understand the style. :) -Bianca  
                return NotFound("Requisition Details not found");
        }

        [HttpGet("viewRequisitionApply")]
        public async Task<ActionResult<IList<RequisitionDetail>>> viewRequisitionApply()
        {
            var result = await _deptService.viewRequisitionApplyRow();
            // if find data then return result else will return a String says Department not found
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                //There are 3 Department alr seeded in DB, so this line should nvr appears. 
                //I put here Just for u to understand the style. :) -Bianca  
                return NotFound("Requisition Details not found");
        }

        [HttpGet("viewRequisition")]
        public async Task<ActionResult<IList<RequisitionDetailsApply>>> viewRequisition([FromBody] Requisition requisition)
        {
            var result = await _deptService.viewRequisitionApply(requisition);
            // if find data then return result else will return a String says Department not found
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                //There are 3 Department alr seeded in DB, so this line should nvr appears. 
                //I put here Just for u to understand the style. :) -Bianca  
                return NotFound("Requisition Details not found");
        }

        #endregion

        #region Basic info-Stationery
        [HttpGet("stationery")]
        public async Task<ActionResult<IList<Requisition>>> GetAllStationery()
        {
            var result = await _deptService.findAllStationeryAsync();
            //var result2 = result.First<Requisition>().Employee;
            // if find data then return result else will return a String says Department not found
            if (result != null)
                //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
                return Ok(result);
            else
                //this help to return a NOTfOUND result, u can customerize the string.
                //There are 3 Department alr seeded in DB, so this line should nvr appears. 
                //I put here Just for u to understand the style. :) -Bianca  
                return NotFound("Requisition Details not found.");
        }
        #endregion

        #region Dept-Head/delegate anthorize + DEPT-info
        [HttpGet("deptPendingReq/{id}")]
        public async Task<ActionResult<IList<Requisition>>> GetPendingRequisitionsByDeptId(int id)
        {
	        var allRequisitionsList = await _deptService.findAllRequsitionsAsync();
	        var allEmployeesList = await _deptService.findAllEmployeesAsync();

	        var allPendingRequisitionsList =
	            allRequisitionsList.Where(x => x.status == "Applied");

	        var allEmployeesUnderDeptList = allEmployeesList.Where(x => x.departmentId == id);

            List<Requisition> allPendingRequisitionsUnderDeptList = new List<Requisition>();

            foreach (Requisition requisition in allPendingRequisitionsList)
            {
	            foreach(Employee employee in allEmployeesUnderDeptList)
	            {
		            if (requisition.EmployeeId == employee.Id)
		            {
                        allPendingRequisitionsUnderDeptList.Add(requisition);
		            }
	            }
            }

            if (allPendingRequisitionsUnderDeptList.Any())
		        //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
		        return Ok(allPendingRequisitionsUnderDeptList);
	        else
		        return NotFound("No pending requisition under this department.");
        }

        [HttpGet("deptPendingReqDetail/{id}")]
        public async Task<ActionResult<IList<RequisitionDetail>>> GetPendingRequisitionsDetailByDeptId(int id)
        {
	        var allRequisitionsList = await _deptService.findAllRequsitionsAsync();
	        var allRequisitionsDetailList = await _deptService.findAllRequsitionDetailAsync();
            var allEmployeesList = await _deptService.findAllEmployeesAsync();

	        var allPendingRequisitionsList =
		        allRequisitionsList.Where(x => x.status == "Applied");

	        var allEmployeesUnderDeptList = allEmployeesList.Where(x => x.departmentId == id);

	        List<Requisition> allPendingRequisitionsUnderDeptList = new List<Requisition>();

	        foreach (Requisition requisition in allPendingRequisitionsList)
	        {
		        foreach (Employee employee in allEmployeesUnderDeptList)
		        {
			        if (requisition.EmployeeId == employee.Id)
			        {
				        allPendingRequisitionsUnderDeptList.Add(requisition);
			        }
		        }
	        }

	        List<RequisitionDetail> allPendingRequisitionsDetailUnderDeptList = new List<RequisitionDetail>();
	        
            foreach (RequisitionDetail requisitionDetail in allRequisitionsDetailList)
	        {
		        foreach (Requisition requisition in allPendingRequisitionsUnderDeptList)
		        {
			        if (requisitionDetail.RequisitionId == requisition.Id) 

                    {
                        allPendingRequisitionsDetailUnderDeptList.Add(requisitionDetail);
			        }
		        }
	        }

	        if (allPendingRequisitionsDetailUnderDeptList.Any())
		        //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
		        return Ok(allPendingRequisitionsDetailUnderDeptList);
	        else
		        return NotFound("No pending requisition detail under this department.");
        }
        
        [HttpGet("deptEmp/{id}")]
        public async Task<ActionResult<IList<Employee>>> GetAllEmployeesByDept(int id)
        {
	        var allEmployeesList = await _deptService.findAllEmployeesAsync();
	        var allEmployeesUnderDeptList = allEmployeesList.Where(x => x.departmentId == id);

            if (allEmployeesUnderDeptList.Any())
	            return Ok(allEmployeesUnderDeptList);
	        else
	            return NotFound("Employees not found.");
        }

        [HttpPost("deptCollection/{id}")]
        public Task<ActionResult<Department>> DeptCollection(
	        [FromBody] List<Department> department, int id)
        {
	        Console.WriteLine("post");
	        Console.WriteLine(id);
	        Console.WriteLine(department[0]);
	        return null;
        }
        #endregion

        #region Dept-Rep
        [HttpGet("disbursementListByDept/{id}")]
        public async Task<ActionResult<IList<DisbursementList>>> GetDisbursementListByDeptId(int id)
        {
	        var allDisbursement = await _clerkService.findAllDisbursementListAsync();

	        var allDisbursementUnderDept =
		        allDisbursement.Where(x => x.DepartmentId == id);

	        if (allDisbursementUnderDept.Any())
		        //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
		        return Ok(allDisbursementUnderDept);
	        else
		        return NotFound("No disbursement list under this department.");
        }

        [HttpGet("disbursementDetailByDept/{id}")]
        public async Task<ActionResult<IList<DisbursementDetail>>> GetDisbursementDetailByDeptId(int id)
        {
	        var allDisbursementList = await _clerkService.findAllDisbursementListAsync();
            var allDisbursementDetail = await _clerkService.findAllDisbursementDetailAsync();

	        var allDisbursementListUnderDept =
		        allDisbursementList.Where(x => x.DepartmentId == id);

	        List<DisbursementDetail> allDisbursementDetailUnderDept = new List<DisbursementDetail>();

	        foreach (DisbursementDetail disbursementDetail in allDisbursementDetail)
	        {
		        foreach (DisbursementList disbursementList in allDisbursementListUnderDept)
		        {
			        if (disbursementDetail.DisbursementListId == disbursementList.id)
			        {
				        allDisbursementDetailUnderDept.Add(disbursementDetail);
			        }
		        }
	        }

            if (allDisbursementDetailUnderDept.Any())
		        //Docs says that Ok(...) will AUTO TRANSFER result into JSON Type
		        return Ok(allDisbursementDetailUnderDept);
	        else
		        return NotFound("No disbursement detail under this department.");
        }
        #endregion

        #region read this before starting
        //this not work Sry Idk details, it is weird. -Bianca
        // GET: api/dept/search?name=ComputerScience
        /*
        [HttpGet("search")]
        public ActionResult<Department> Search(string name)
        {
            Regex r = new Regex(@"(?!^)(?=[A-Z])");
            String nameWithSpace = r.Replace(name, "");
            var dept = _deptService.findDepartmentByName(nameWithSpace);
            if (dept == null || name == null)
            {
                return null;
            }
            return dept;
        }*/

        /* We should use async methods here to improve efficiency
         * However, Here is a sample code for sync method -  getDeptById for u to get familiar with
         * public ActionResult<Department> GetDepartmentById(int id)
        {
            return  _deptService.findDepartmentById(id);
            // u also need findDeptById in your service layer and repo layer 
            (DO NOT FORGET INTERFACE and AddScoped<...> for BOTH repo and service)
        }*/
        #endregion

        #region stock Clerk-fulfill req
        [HttpGet("retrieval")]
        public async Task<ActionResult<IList<Requisition>>> GetAllPendingRequisitions()
        {
            var result = await _deptService.findAllRequsitionsAsync();

            var result_filtered = result.Where(x => x.status != "Delivered");
            foreach (var x in result_filtered)
            {
                Console.WriteLine(x.Id);
            }
            var result_filtered2 = result_filtered.Where(x => x.status != "Declined");
            foreach (var x in result_filtered2)
            {
                Console.WriteLine(x.Id);
            }

            if (result_filtered2 != null)
                //convert to json file
                return Ok(result_filtered2);
            else
                //in case there is nothing to process
                return NotFound("No pending requistions.");
        }

        
        #endregion

    }
}