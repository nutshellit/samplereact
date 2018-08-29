import React, { Component } from 'react';
import RespondQuestionnaireSection from './RespondQuestionnaireSection';
class RespondQuestionnaire extends Component {
    constructor(props) {
        super(props);
        this.state = { questionnaire: {}, questionItems: {}, loading: true };
        let url = `/api/questionnaire/getquestionnaire/${props.match.params.questionnairename}`;
        fetch(url)
            .then(response => response.json())
            .then(data => {
                this.setupState(data);
            });
        //this.handleQuestionChange = this.handleQuestionChange.bind(this);
    }

    handleQuestionChange(change)
    {
        //see http://jsbin.com/rozayedibe/edit?js,console,output
        //see https://medium.freecodecamp.org/reactjs-pass-parameters-to-event-handlers-ca1f5c422b9
        console.log(change);
        let existingQuestionItems = this.state.questionItems;
        let item = existingQuestionItems.get(change.QuestionId);
        item.response = change.NewValue;
        existingQuestionItems.delete(change.QuestionId);
        existingQuestionItems.set(change.QuestionId,item);
        let newQuestionItems = this.updateResponse(this.state.questionnaire, existingQuestionItems);
        this.setState({  questionItems: newQuestionItems });
    }

    updateResponse(data, questionItems) {
        console.log(questionItems);
        let updatedQuestionItems = new Map();
        data.sections.forEach(section => {
            section.questions.forEach(question => {
                let existingQuestionItem = questionItems.get(question.questionId);
                if (question.parentQuestionId != null) {
                    let parentQuestionResponse = questionItems.get(question.parentQuestionId);
                    console.log(`#1 - Response ${parentQuestionResponse.response} - ParentOptId ${question.parentQuestionOptionId} `);

                    if (parseInt(parentQuestionResponse.response, 10) === question.parentQuestionOptionId) {
                        console.log('#2');
                        let qr = { questionId: existingQuestionItem.questionId, isHidden: false, response: existingQuestionItem.response };
                        updatedQuestionItems.set(question.questionId, qr)
                    }
                    else {
                        console.log('#3');
                        let qr = { questionId: existingQuestionItem.questionId, isHidden: true, response: existingQuestionItem.response };
                        updatedQuestionItems.set(question.questionId, qr);
                    }
                }
                else {
                    updatedQuestionItems.set(question.questionId, existingQuestionItem);
                }
            });
        });
        console.log(updatedQuestionItems);
        return updatedQuestionItems;
    }

    setupState(data) {
        var questionItems = new Map();
        data.sections.forEach(section => {
            section.questions.forEach(q => {
                let qr = { questionId: q.questionId, isHidden: false, response: '' };
                questionItems.set(q.questionId, qr);
            })
        });
        questionItems = this.updateResponse(data, questionItems);
        this.setState({ questionnaire: data, questionItems: questionItems, loading: false });
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading....</em></p>
            : <div><h2>Questionnaire : {this.state.questionnaire.name} </h2>
                {this.state.questionnaire.sections.map(s =>
                 <RespondQuestionnaireSection
                    key={s.sectionName}
                    section={s}
                    questionItems={this.state.questionItems} 
                    onQuestionChange={ c => { 
                       this.handleQuestionChange(c);
                     } }
                    />)}

            </div>
        return (
            <div>
                {contents}
            </div>
        );
    }
}

export default RespondQuestionnaire;