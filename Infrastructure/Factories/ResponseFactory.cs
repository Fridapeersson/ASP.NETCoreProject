//using Infrastructure.Models;

//namespace Infrastructure.Factories;

//public class ResponseFactory
//{
//    public static ResponseResult OK()
//    {
//        return new ResponseResult
//        {
//            Message = "Succeeded",
//            StatusCode = StatusCodes.OK
//        };
//    }

//    public static ResponseResult OK(object obj, string? message = null)
//    {
//        return new ResponseResult
//        {
//            Result = obj,
//            Message = message,
//            StatusCode = StatusCodes.OK
//        };
//    }

//    public static ResponseResult OK(string? message = null)
//    {
//        return new ResponseResult
//        {
//            Message = message,
//            StatusCode = StatusCodes.OK
//        };
//    }


//    public static ResponseResult ERROR(string message)
//    {
//        return new ResponseResult
//        {
//            Message = message,
//            StatusCode = StatusCodes.ERROR
//        };
//    }

//    public static ResponseResult NOT_FOUND(string? message = null)
//    {
//        return new ResponseResult
//        {
//            Message = message,
//            StatusCode = StatusCodes.NOT_FOUND
//        };
//    }

//    public static ResponseResult EXISTS(string? message = null)
//    {
//        return new ResponseResult
//        {
//            Message = message ?? "Already exists",
//            StatusCode = StatusCodes.EXISTS
//        };
//    }
//}
