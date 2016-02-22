
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

app_router.on("route:allQuestions", function (actions) {
    allQuestionsView.show();
    singleQuestionView.hide();
    aboutView.hide();
});

app_router.on("route:renderSingleQuestion", function (id) {
    allQuestionsView.hide();
    singleQuestionView.show(id);
    aboutView.hide();
});

app_router.on("route:renderAbout", function () {
    allQuestionsView.hide();
    singleQuestionView.hide();
    aboutView.show();
});
Backbone.history.start();

$(".navbar a").click(function (ev) {
    $(".navbar .active").removeClass("active");
    $(ev.target).closest("li").addClass("active");
});