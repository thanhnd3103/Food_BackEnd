﻿namespace Common.Constants
{
    public static class Messages
    {
        public static class General
        {
            public const string CRITICAL_UNIDENTIFIED_ERROR = "Lỗi không xác định";
            public const string MODEL_STATE_INVALID = "Nhập thiếu hoặc sai định dạng dữ liệu, vui lòng kiểm tra kết quả trả về";
            public const string NO_DATA_ERROR = "Không có dữ liệu trùng khớp";
        }

        public static class AuthController
        {
            public const string PASSWORD_NOT_MATCHED = "Mật khẩu không khớp, vui lòng kiểm tra lại";
            public const string EMAIL_ALREADY_EXISTED = "Email đã được sử dụng trong hệ thống";

            public const string REGISTER_SUCCESS = "Đăng ký thành công";

            public const string LOGIN_FAILED = "Nhập sai tài khoản hay mật khẩu";

            public const string LOGIN_SUCCESS = "Đăng nhập thành công";
            public const string LOGIN_INTERNAL_ERROR = "Không tìm thấy tài khoản đã đăng nhập";
        }

        public static class DishMessage
        {
            public const string LIST_DISHES_MESSAGE_SUCCESS = "Một dãy các món ăn";
            public const string CREATE_DISH_MESSAGE_SUCCESS = "Món ăn được tạo thành công";
            public const string GET_DISH_SUCCESS = "Lấy món ăn thành công";
            public const string NO_CONTENT = "Không có món ăn thuộc tag này, hoặc tag không tồn tại";
        }

        public static class OrderMessage
        {
            public const string PASS_WRONG_ID = "Truyền Id sai";
            public const string ORDER_SUCCESS = "Tạo order thành công";
        }
    }
}
