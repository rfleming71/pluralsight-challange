var QuestionModel = Backbone.Model.extend({
    idAttribute: "QuestionId",
    defaults: {
        QuestionId: undefined,
        Text: undefined,
        Answer: undefined,
        Destractors: []
    },
    initialize: function (options) {
        this.urlRoot = options.urlRoot;
    }
});

var QuestionCollection = Backbone.Collection.extend({
    model: QuestionModel,
    initialize: function (options) {
        this.url = options.url;
    }
});