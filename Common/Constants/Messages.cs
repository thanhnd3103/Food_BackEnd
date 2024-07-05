namespace Common.Constants
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
            public const string CREATE_UPDATE_DISH_MESSAGE_SUCCESS = "Món ăn được cập nhật thành công";
            public const string CREATE_UPDATE_DISH_MESSAGE_SUCCESS_NO_TAG = "Món ăn được cập nhật thành công, nhưng không có tag";
            public const string GET_DISH_SUCCESS = "Lấy món ăn thành công";
            public const string NO_CONTENT = "Không có món ăn hợp lệ để thực hiện hành động này.";
            public const string DUPLICATED_DISH = "Món ăn đã tồn tại";
            public const string INVALID_TAG = "Chứa tag không hợp lệ";
        }

        public static class OrderMessage
        {
            public const string PASS_WRONG_ID = "Truyền Id sai";
            public const string ORDER_SUCCESS = "Tạo order thành công";
            public const string LIST_ORDER_SUCCESS = "List orders của nhà hàng";
        }

        public static class AccountMessage
        {
            public const string GET_ACCOUNT_BY_ID_SUCCESS = "Trả về dữ liệu của người dùng thnh công";
            public const string UPDATE_ACCOUNT_SUCCESS = "Cập nhật dữ liệu người dùng thành công";
        }

        public static class TagMessage
        {
            public const string GET_TAGS_SUCCESS = "Dữ liệu của tags";
        }

        public static class OrderDetailMessage
        {
            public const string GET_ORDER_DETAIL_BY_ID_SUCCESS = "Dữ liệu order detail";
        }
    }
}
