<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentPage.aspx.cs" Inherits="MandarinQuest.StudentPage" Async="true" %>

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

.chatButton{
    position:fixed;
    right:24px;
    bottom:24px;
    background:linear-gradient(135deg,#a11212,#d32f2f);
    color:#fff;
    padding:14px 20px;
    border-radius:999px;
    cursor:pointer;
    box-shadow:0 10px 25px rgba(161,18,18,0.35);
    font-weight:600;
    z-index:999;
    transition:all 0.25s ease;
}

.chatButton:hover{
    transform:translateY(-2px);
    box-shadow:0 14px 28px rgba(161,18,18,0.42);
}

.chatPanel{
    position:fixed;
    right:24px;
    bottom:84px;
    width:380px;
    max-width:calc(100vw - 30px);
    background:#fff;
    border-radius:20px;
    overflow:hidden;
    box-shadow:0 18px 45px rgba(0,0,0,0.22);
    display:none;
    z-index:1000;
    border:1px solid #f0dede;
}

.chatHeader{
    display:flex;
    justify-content:space-between;
    align-items:flex-start;
    gap:10px;
    padding:16px 18px;
    background:linear-gradient(135deg,#a11212,#c62828);
    color:white;
}

.chatTitle{
    font-size:17px;
    font-weight:700;
}

.chatSubtitle{
    font-size:12px;
    opacity:0.9;
    margin-top:4px;
}

.chatClose{
    background:rgba(255,255,255,0.15);
    color:white;
    border:none;
    width:32px;
    height:32px;
    border-radius:50%;
    cursor:pointer;
    font-size:14px;
}

.chatClose:hover{
    background:rgba(255,255,255,0.25);
}

.chatBox{
    height:340px;
    overflow-y:auto;
    padding:16px;
    background:#fff8f8;
    border-bottom:1px solid #eee;
}

.msg{
    display:flex;
    margin-bottom:12px;
}

.msg.user{
    justify-content:flex-end;
}

.msg.ai{
    justify-content:flex-start;
}

.bubble{
    max-width:82%;
    padding:12px 14px;
    border-radius:16px;
    line-height:1.5;
    font-size:14px;
    box-shadow:0 4px 10px rgba(0,0,0,0.06);
    word-wrap:break-word;
}

.userBubble{
    background:#a11212;
    color:white;
    border-bottom-right-radius:6px;
}

.aiBubble{
    background:white;
    color:#333;
    border:1px solid #f1d4d4;
    border-bottom-left-radius:6px;
}

.chatInputArea{
    display:flex;
    gap:10px;
    padding:14px;
    background:white;
    align-items:flex-end;
}

.chatInput{
    flex:1;
    resize:none;
    padding:12px 14px;
    border:1px solid #e3c8c8;
    border-radius:14px;
    font-family:'Segoe UI';
    font-size:14px;
    outline:none;
    min-height:46px;
    box-sizing:border-box;
}

.chatInput:focus{
    border-color:#c62828;
    box-shadow:0 0 0 3px rgba(198,40,40,0.10);
}

.chatSendBtn{
    background:linear-gradient(135deg,#a11212,#d32f2f);
    color:white;
    border:none;
    border-radius:14px;
    padding:12px 18px;
    cursor:pointer;
    font-weight:600;
    min-width:82px;
}

.chatSendBtn:hover{
    filter:brightness(1.05);
}

@media (max-width: 600px){
    .chatPanel{
        right:10px;
        left:10px;
        width:auto;
        bottom:78px;
    }

    .chatButton{
        right:12px;
        bottom:12px;
    }

    .chatBox{
        height:300px;
    }
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

    function openChat() {
        document.getElementById("chatPanel").style.display = "block";
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
    💬 AI Tutor
</div>

<div class="chatPanel" id="chatPanel">
    <div class="chatHeader">
        <div>
            <div class="chatTitle">AI Mandarin Tutor</div>
            <div class="chatSubtitle">Ask words, pinyin, grammar, and examples</div>
        </div>
        <button type="button" class="chatClose" onclick="toggleChat()">✕</button>
    </div>

    <div class="chatBox" id="chatBox">
        <asp:Literal ID="litChatHistory" runat="server"></asp:Literal>

        <asp:Panel ID="pnlWelcome" runat="server">
            <div class="msg ai">
                <div class="bubble aiBubble">
                    你好! I’m your Mandarin tutor. Try asking:
                    <br /><br />
                    • How do I say “good morning” in Mandarin?
                    <br />
                    • Give me 3 beginner Mandarin phrases
                    <br />
                    • Explain “xie xie” with pinyin
                </div>
            </div>
        </asp:Panel>
    </div>

    <div class="chatInputArea">
        <asp:TextBox
            ID="txtQuestion"
            runat="server"
            CssClass="chatInput"
            TextMode="MultiLine"
            Rows="2"
            placeholder="Type your question here..."></asp:TextBox>

        <asp:Button
            ID="btnAskAI"
            runat="server"
            Text="Send"
            CssClass="chatSendBtn"
            OnClick="btnAskAI_Click" />
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