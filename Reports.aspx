<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="MandarinQuest.Reports" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: #f3f4f6;
            margin: 0;
            padding: 40px 20px;
        }

        .container {
            max-width: 1100px;
            margin: auto;
        }

        h1, h2 {
            text-align: center;
            color: #b91c1c;
        }

        h1 {
            margin-bottom: 40px;
            font-size: 2.8rem;
        }

        h2 {
            margin-top: 40px;
            margin-bottom: 20px;
            font-size: 2rem;
        }

        .cards {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            gap: 20px;
        }

        .card {
            background: #ffffff;
            padding: 30px 20px;
            border-radius: 12px;
            box-shadow: 0 8px 20px rgba(0,0,0,0.08);
            flex: 1 1 220px;
            max-width: 250px;
            text-align: center;
            transition: transform 0.3s, box-shadow 0.3s;
        }

        .card:hover {
            transform: translateY(-5px);
            box-shadow: 0 12px 25px rgba(0,0,0,0.15);
        }

        .card h2 {
            font-size: 3rem;
            margin: 15px 0;
            color: #dc2626;
        }

        .card p {
            font-size: 1rem;
            color: #4b5563;
            margin: 0;
        }

        .btn {
            display: inline-block;
            padding: 10px 20px;
            border: none;
            border-radius: 8px;
            background: #dc2626;
            color: #fff;
            cursor: pointer;
            font-size: 1rem;
            margin-bottom: 30px;
            transition: background 0.3s, transform 0.2s;
        }

        .btn:hover {
            background: #b91c1c;
            transform: translateY(-2px);
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        table, th, td {
            border: 1px solid #ddd;
        }

        th, td {
            padding: 10px;
            text-align: center;
        }

        th {
            background-color: #dc2626;
            color: white;
        }

        .progress-bar {
            height: 18px;
            background-color: #f3f3f3;
            border-radius: 8px;
            overflow: hidden;
        }

        .progress {
            height: 100%;
            background-color: #dc2626;
            text-align: right;
            padding-right: 5px;
            color: white;
            font-size: 0.8rem;
            line-height: 18px;
        }

        @media (max-width: 768px) {
            .cards {
                flex-direction: column;
                align-items: center;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>View Reports</h1>

            <asp:Button ID="btnBack" runat="server"
                        Text="← Back to Admin"
                        CssClass="btn"
                        OnClick="btnBack_Click" />

            <div class="cards">
                <div class="card">
                    <h2><asp:Label ID="lblTeachers" runat="server" Text="0"></asp:Label></h2>
                    <p>Total Teachers</p>
                </div>

                <div class="card">
                    <h2><asp:Label ID="lblStudents" runat="server" Text="0"></asp:Label></h2>
                    <p>Total Students</p>
                </div>

                <div class="card">
                    <h2><asp:Label ID="lblLessons" runat="server" Text="0"></asp:Label></h2>
                    <p>Total Lessons</p>
                </div>

                <div class="card">
                    <h2><asp:Label ID="lblMaterials" runat="server" Text="0"></asp:Label></h2>
                    <p>Total Learning Materials</p>
                </div>
            </div>

            <h2>New Registrations (Monthly)</h2>
            <asp:GridView ID="gvRegistrations" runat="server" AutoGenerateColumns="false" CssClass="table">
                <Columns>
                    <asp:BoundField DataField="Month" HeaderText="Month" />
                    <asp:BoundField DataField="Count" HeaderText="New Students Registered" />
                </Columns>
            </asp:GridView>

            <h2>Course Progress</h2>
            <asp:GridView ID="gvCourseProgress" runat="server" AutoGenerateColumns="false" CssClass="table">
                <Columns>
                    <asp:BoundField DataField="Lesson" HeaderText="Lesson" />
                    <asp:BoundField DataField="StudentsCompleted" HeaderText="Students Completed" />
                    <asp:TemplateField HeaderText="Progress">
                        <ItemTemplate>
                            <div class="progress-bar">
                                <div class="progress" style='width:<%# Eval("Percentage") %>%'>
                                    <%# Eval("Percentage") %>%
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </div>
    </form>
</body>
</html>