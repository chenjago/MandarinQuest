<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentPage.aspx.cs" Inherits="MandarinQuest.StudentPage" %>

<!DOCTYPE html>
<html>
<head runat="server">

<title>MandarinQuest Student</title>

<style>

/* PAGE */

body{
margin:0;
font-family:'Segoe UI';
background:#f6efe6;
}

/* NAVBAR */

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

/* HERO */

.hero{
height:200px;
background:#b11a1a;
color:white;
display:flex;
flex-direction:column;
align-items:center;
justify-content:center;
font-size:32px;
}

.hero small{
font-size:16px;
margin-top:10px;
}

/* LEVEL TITLE */

.sectionTitle{
text-align:center;
margin-top:40px;
font-size:24px;
color:#a11212;
}

/* LEVEL CARDS */

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

.card.locked{
background:#ddd;
color:#777;
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

/* SESSION SECTION */

.sessions{
margin-top:50px;
padding:30px;
}

.sessionTitle{
text-align:center;
font-size:24px;
color:#a11212;
margin-bottom:25px;
}

/* SESSION GRID */

.sessionGrid{
display:grid;
grid-template-columns:repeat(auto-fit,minmax(260px,1fr));
gap:20px;
max-width:1000px;
margin:auto;
}

/* SESSION CARD */

.sessionCard{
background:white;
border-radius:12px;
padding:20px;
box-shadow:0 6px 14px rgba(0,0,0,0.15);
transition:0.3s;
}

.sessionCard:hover{
transform:translateY(-5px);
}

.sessionLevel{
font-size:13px;
color:#888;
margin-bottom:6px;
}

.sessionName{
font-size:18px;
font-weight:600;
margin-bottom:8px;
}

.sessionDate{
font-size:14px;
color:#555;
margin-bottom:15px;
}

.copyBtn{
background:#a11212;
color:white;
border:none;
padding:8px 12px;
border-radius:6px;
cursor:pointer;
font-size:13px;
}

.copyBtn:hover{
background:#d32f2f;
}

.noSession{
text-align:center;
color:#777;
margin-top:10px;
}

/* CHAT BUTTON */

.chatButton{
position:fixed;
bottom:20px;
right:20px;
background:#a11212;
color:white;
padding:12px 16px;
border-radius:30px;
cursor:pointer;
box-shadow:0 4px 10px rgba(0,0,0,0.3);
}

/* CHAT PANEL */

.chatPanel{
position:fixed;
bottom:80px;
right:20px;
width:320px;
background:white;
border-radius:10px;
box-shadow:0 4px 12px rgba(0,0,0,0.3);
display:none;
}

.chatHeader{
background:#a11212;
color:white;
padding:10px;
border-top-left-radius:10px;
border-top-right-radius:10px;
}

.chatBox{
height:220px;
overflow-y:auto;
padding:10px;
border-bottom:1px solid #ddd;
}

/* CUSTOMER SERVICE BUTTON */

.csButton{
position:fixed;
bottom:20px;
left:20px;
background:#a11212;
color:white;
padding:14px;
border-radius:50%;
cursor:pointer;
box-shadow:0 4px 10px rgba(0,0,0,0.3);
font-size:20px;
}

/* CS PANEL */

.csPanel{
position:fixed;
bottom:80px;
left:20px;
width:260px;
background:white;
border-radius:10px;
box-shadow:0 4px 12px rgba(0,0,0,0.3);
display:none;
padding:15px;
}

</style>

<script>

    function toggleChat() {

        var panel = document.getElementById("chatPanel");

        if (panel.style.display == "none" || panel.style.display == "")
            panel.style.display = "block";
        else
            panel.style.display = "none";
    }

    function toggleCS() {

        var panel = document.getElementById("csPanel");

        if (panel.style.display == "none" || panel.style.display == "")
            panel.style.display = "block";
        else
            panel.style.display = "none";
    }

</script>

</head>

<body>

<form id="form1" runat="server">

<div class="navbar">

<div><b>MandarinQuest</b></div>

<div class="menu">

<asp:Button ID="btnMaterials" runat="server" Text="Materials" OnClick="btnMaterials_Click"/>
<asp:Button ID="btnProgress" runat="server" Text="Progress" OnClick="btnProgress_Click"/>
<asp:Button ID="btnProfile" runat="server" Text="Profile" OnClick="btnProfile_Click"/>
<asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click"/>

</div>

</div>

<div class="hero">
<div>Welcome to MandarinQuest</div>
<small>Your journey to learning Mandarin starts here.</small>
</div>


<div class="sectionTitle">
Choose Your Learning Level
</div>


<div class="cards">

<asp:Repeater ID="rptLevels" runat="server">

<ItemTemplate>

<div class='card <%# (bool)Eval("Locked") ? "locked" : "" %>'>

<div class="levelTitle"><%# Eval("LevelName") %></div>

<p><%# Eval("Description") %></p>

<asp:Button
runat="server"
Text="Start Learning"
CssClass="levelBtn"
CommandArgument='<%# Eval("LevelID") %>'
OnCommand="Level_Click"
Visible='<%# !(bool)Eval("Locked") %>' />

<asp:Label
runat="server"
Text="🔒 Locked"
Visible='<%# (bool)Eval("Locked") %>' />

</div>

</ItemTemplate>

</asp:Repeater>

</div>


<!-- UPCOMING SESSIONS -->

<div class="sessions">

<div class="sessionTitle">
Upcoming Sessions
</div>

<div class="sessionGrid">

<asp:Repeater ID="rptSessions" runat="server">

<ItemTemplate>

<div class="sessionCard">

<div class="sessionLevel">
Level: <%# Eval("LevelName") %>
</div>

<div class="sessionName">
<%# Eval("SessionTitle") %>
</div>

<div class="sessionDate">
📅 <%# Eval("SessionDate","{0:dd MMM yyyy}") %>
</div>

<button
type="button"
class="copyBtn"
onclick="navigator.clipboard.writeText('<%# Eval("SessionLink") %>');alert('Session link copied!');">
Copy Link
</button>

</div>

</ItemTemplate>

</asp:Repeater>

</div>

<asp:Label 
ID="lblNoSession" 
runat="server" 
Text="No upcoming sessions available."
CssClass="noSession"
Visible="false"/>

</div>


<!-- AI CHAT -->

<div class="chatButton" onclick="toggleChat()">
AI Tutor
</div>

<div class="chatPanel" id="chatPanel">

<div class="chatHeader">
AI Mandarin Tutor
</div>

<div class="chatBox">
<asp:Literal ID="litChatHistory" runat="server"></asp:Literal>
</div>

<div style="padding:10px;">

<asp:TextBox ID="txtQuestion" runat="server" Width="70%"></asp:TextBox>

<asp:Button
ID="btnAskAI"
runat="server"
Text="Send"
CssClass="levelBtn"
OnClick="btnAskAI_Click"/>

</div>

</div>


<!-- CUSTOMER SERVICE -->

<div class="csButton" onclick="toggleCS()">💬</div>

<div class="csPanel" id="csPanel">

<b>Customer Support</b>

<p>Email: support@mandarinquest.com</p>

<p>Phone: +60 12-345 6789</p>

</div>


</form>

</body>
</html>