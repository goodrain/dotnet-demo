using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace aspnetapp.Utils {
   public class ResultUtil {
      public static JsonResult GetFailureResult(int code, string msg) {
        JsonResult result = new JsonResult(new Result(msg,null,null));
        result.StatusCode = code;
        return result;
      }
       public static JsonResult GetSuccessResult(System.Object bean) {
        JsonResult result = new JsonResult(new Result("success",bean, null));
        return result;
      }
      public static JsonResult GetSuccessResult<T>(List<T> list) {
        JsonResult result = new JsonResult(new Result("success", null, list));
        return result;
      }
   }
   public class Result
   {
       public string Msg {get; set;}
       public System.Object Bean {get; set;}
       public System.Object List {get; set;}

       public Result(string msg, System.Object bean, System.Object list){
            Msg = msg;
            Bean = bean;
            List = list;
       }
   }
}