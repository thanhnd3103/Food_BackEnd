﻿namespace Common.Constants
{
    public static class Messages
    {
        public static class General
        {
            public const string CRITICAL_UNIDENTIFIED_ERROR = "Lỗi không xác định";
            public const string MODEL_STATE_INVALID = "Nhập thiếu hoặc sai định dạng dữ liệu, vui lòng kiểm tra kết quả trả về";
        }

        public static class AuthController
        {
            public const string PASSWORD_NOT_MATCHED = "Mật khẩu không khớp, vui lòng kiểm tra lại";
            public const string EMAIL_ALREADY_EXISTED = "Email đã được sử dụng trong hệ thống";

            public const string REGISTER_SUCCESS = "Đăng ký thành công";

            public const string LOGIN_FAILED = "Nhập sai tài khoản hay mật khẩu";

            public const string LOGIN_SUCCESS = "Đăng nhập thành công";
        }

        public static class DishMessage
        {
            public const string List_Dishes_Message_Success = "Một dãy các món ăn";
        }
    }
}
