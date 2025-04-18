# 🏠 Rent Nest System

**Rent Nest System** là một nền tảng quản lý cho thuê được xây dựng bằng ASP.NET Core MVC. Hệ thống cho phép người dùng tìm kiếm, đăng và quản lý tin đăng bất động sản một cách hiệu quả với nhiều tính năng tiện lợi.

---

## 🚀 Features

- 🔐 Google OAuth 2.0 Login
- 👨‍💼 Phân quyền đăng nhập: Admin / Client
- 📄 Đăng, chỉnh sửa và quản lý bài đăng cho thuê
- ✅ Admin duyệt bài viết trước khi hiển thị
- 🔔 Hệ thống thông báo real-time (SignalR)
- 🔍 Tìm kiếm + phân trang thông minh
- 📍 Tích hợp Google Maps API
- 📦 Upload hình ảnh bất động sản
- 📊 Trang Dashboard cho Admin
- 💬 Quản lý feedback người dùng
- 🕵️‍♂️ Session & Authentication Security

---

## 🛠️ Công nghệ sử dụng

| Layer          | Stack                                         |
| -------------- | --------------------------------------------- |
| Frontend       | HTML/CSS, JavaScript, Bootstrap, jQuery, Swal |
| Backend        | ASP.NET Core MVC, Entity Framework Core       |
| Database       | MS SQL Server                                 |
| Authentication | Google OAuth 2.0, Session Auth                |
| Real-time      | SignalR                                       |
| API            | Google Maps API                               |

---

## ⚙️ Hướng dẫn cài đặt & chạy

### 1. Clone repository

git clone https://github.com/mtunsnef/Rent_Nest_System.git
cd Rent_Nest_System

### 2. Cài đặt database

Cấu hình chuỗi kết nối trong file appsettings.json, sau đó chạy lệnh sau để tạo database từ migration:
bashdotnet ef database update

### 3. Cài đặt Google OAuth

Tạo một project tại Google Developers Console và lấy thông tin ClientId, ClientSecret.
Thêm thông tin này vào file appsettings.Development.json (file này không được push lên GitHub)

### 4. Chạy SQL

Chạy Scripts/database.sql trong SQL Server trước chạy bắt đầu dự án

### 5. Chạy ứng dụng

dotnet run --project RentNest.Web

## 🔐 Ghi chú bảo mật

Mọi thông tin nhạy cảm như OAuth secrets phải được đặt trong appsettings.Development.json hoặc dùng biến môi trường
❌ Tuyệt đối không được commit lên GitHub

## 🤝 Đóng góp

Pull request luôn được chào đón. Vui lòng mở issue nếu bạn muốn thảo luận tính năng mới hoặc sửa lỗi.
