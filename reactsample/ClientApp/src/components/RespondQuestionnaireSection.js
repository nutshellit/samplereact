import React, { Component } from 'react';
import RespondQuestionnaireTextbox from './RespondQuestionnaireTextbox';
import RespondQuestionnaireRadio from './RespondQuestionnaireRadio';
class RespondQuestionnaireSection extends Component {
    constructor(props) {
        super(props);
        this.state = {  };
    }
    render() {
        return (

            <div>
            <h3>Section : {this.props.section.sectionName} </h3>
            
                { this.props.section.questions.map(q =>  {
                   
                   if(q.questionType === 1){
                     return  <RespondQuestionnaireTextbox 
                                key={q.questionId} 
                                question={q} 
                                questionItems={this.props.questionItems}
                                onQuestionChange={this.props.onQuestionChange} />
                   } 
                   else if (q.questionType === 2)
                   {
                       return <RespondQuestionnaireRadio 
                                    key={q.questionId} 
                                    question={q} 
                                    questionItems={this.props.questionItems}
                                    onQuestionChange={this.props.onQuestionChange} />
                   }
                   else{
                       return <div>Invalid.....</div>
                   }
                    
                }) }
            
            </div>
        );
    }
}

export default RespondQuestionnaireSection;