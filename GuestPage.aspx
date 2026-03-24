<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuestPage.aspx.cs" Inherits="MandarinQuest.GuestPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
<title>MandarinQuest</title>

<style>
body{
    margin:0;
    font-family:'Segoe UI';
    background:#f6efe6;
}

.navbar{
    background:#a11212;
    color:white;
    padding:15px;
    display:flex;
    justify-content:space-between;
    align-items:center;
}

.menu input[type=submit]{
    margin-left:10px;
    padding:8px 14px;
    border:none;
    cursor:pointer;
    background:white;
    color:#a11212;
    border-radius:5px;
    font-weight:bold;
}

.menu input[type=submit]:hover{
    background:#ffdede;
}

.hero{
    height:220px;
    background:#b11a1a;
    color:white;
    display:flex;
    flex-direction:column;
    align-items:center;
    justify-content:center;
    font-size:34px;
}

.hero small{
    font-size:16px;
    margin-top:10px;
}

.sectionTitle{
    text-align:center;
    margin-top:40px;
    font-size:24px;
    color:#a11212;
}

.cards{
    display:flex;
    justify-content:center;
    flex-wrap:wrap;
    margin-top:20px;
}

.card{
    background:white;
    width:260px;
    padding:25px;
    margin:15px;
    border-radius:10px;
    box-shadow:0 4px 10px rgba(0,0,0,0.15);
    text-align:center;
}

.levelTitle{
    font-size:20px;
    color:#a11212;
    margin-bottom:10px;
}

.levelBtn{
    margin-top:10px;
    background:#a11212;
    color:white;
    border:none;
    padding:8px 14px;
    border-radius:5px;
    cursor:pointer;
}

.levelBtn:hover{
    background:#d32f2f;
}

.modal{
    position:fixed;
    top:0;
    left:0;
    width:100%;
    height:100%;
    background:rgba(0,0,0,0.5);
    display:none;
    justify-content:center;
    align-items:center;
}

.modalContent{
    background:white;
    width:320px;
    padding:30px;
    border-radius:10px;
    text-align:center;
    position:relative;
}

.closeBtn{
    position:absolute;
    top:10px;
    left:12px;
    font-size:18px;
    cursor:pointer;
}

.modalBtn{
    margin-top:10px;
    width:100%;
    padding:10px;
    border:none;
    border-radius:6px;
    background:#a11212;
    color:white;
    cursor:pointer;
}

.modalBtn:hover{
    background:#d32f2f;
}

</style>

<script>
    function showRegisterNotice() {
        document.getElementById("loginModal").style.display = "flex";
    }

    function closeModal() {
        document.getElementById("loginModal").style.display = "none";
    }
</script>

</head>
<body>

<form id="form1" runat="server">

<div class="navbar">
    <div><b>MandarinQuest</b></div>
    <div class="menu">
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click"/>
        <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click"/>
    </div>
</div>

<div class="hero">
    <div>Welcome to MandarinQuest</div>
    <small>Learn Mandarin step by step through interactive lessons</small>
</div>

<div class="sectionTitle">
Choose Your Learning Level
</div>

<div class="cards">

    <div class="card">
        <div class="levelTitle">Beginner</div>
        <p>Start your Mandarin journey with basic vocabulary and greetings.</p>
        <button type="button" class="levelBtn" onclick="showRegisterNotice()">
            Start Learning
        </button>
    </div>

    <div class="card">
        <div class="levelTitle">Intermediate</div>
        <p>Improve your Mandarin with real conversations and grammar.</p>
        <button type="button" class="levelBtn" onclick="showRegisterNotice()">
            Start Learning
        </button>
    </div>

    <div class="card">
        <div class="levelTitle">Advanced</div>
        <p>Master fluent Mandarin and communicate confidently.</p>
        <button type="button" class="levelBtn" onclick="showRegisterNotice()">
            Start Learning
        </button>
    </div>

</div>

<div class="sectionTitle">
Available Learning Materials
</div>

<div class="cards">
    <div class="card">
        <div class="levelTitle">Learning Materials</div>
        <p>Access a collection of Mandarin learning materials and resources.</p>
        <asp:Button ID="btnMaterialsCard" runat="server" Text="View Materials"
                    CssClass="levelBtn" OnClick="btnMaterialsCard_Click" />
    </div>
</div>

<!-- LOGIN NOTICE MODAL -->
<div class="modal" id="loginModal">
    <div class="modalContent">
        <div class="closeBtn" onclick="closeModal()">✖</div>
        <h3>Access Restricted</h3>
        <p>You must register or login first to access lessons.</p>
        <asp:Button ID="btnRegisterModal" runat="server" Text="Register" CssClass="modalBtn" OnClick="btnRegister_Click" />
        <asp:Button ID="btnLoginModal" runat="server" Text="Login" CssClass="modalBtn" OnClick="btnLogin_Click" />
    </div>
</div>

</form>
</body>
</html>