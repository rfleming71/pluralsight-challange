
var AppRouter = Backbone.Router.extend({
    routes: {
        "question/:id": "renderSingleQuestion",
        "question": "renderSingleQuestion",
        "about": "renderAbout",
        "*actions": "allQuestions"
    }
});

var apiUrlBase = "api/Question";

var app_router = new AppRouter;
var allQuestionsView = new AllQuestionsView({ url: apiUrlBase, el: $("#allQuestions") });
var singleQuestionView = new SingleQuestionView({ url: apiUrlBase, el: $("#singleQuestion") });
var aboutView = $("#about");

var setActiveNavBtn = function (id) {
    $(".navbar .active").removeClass("active");
    $("#" + id).addClass("active");
}

app_router.on("route:allQuestions", function (actions) {
    setActiveNavBtn("allQuestionsBtn");
    allQuestionsView.show();
    singleQuestionView.hide();
    aboutView.hide();
});

app_router.on("route:renderSingleQuestion", function (id) {
    if (id == null) {
        setActiveNavBtn("createNewBtn");
    }
    allQuestionsView.hide();
    singleQuestionView.show(id);
    aboutView.hide();
});

app_router.on("route:renderAbout", function () {
    setActiveNavBtn("aboutBtn");
    allQuestionsView.hide();
    singleQuestionView.hide();
    aboutView.show();
});
Backbone.history.start();
