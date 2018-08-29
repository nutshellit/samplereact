import React, {Component} from 'react';
import {Link} from 'react-router-dom';
export class ListQuestionnaires extends Component{
    constructor(props){
        super(props);
        this.state = {questionnaires:[], loading:true};
        fetch('/api/questionnaire/getall')
            .then(response => response.json())
            .then(data => {
                this.setState({ questionnaires: data, loading: false });
            });
    }

    static renderQuestionnaires(questionnaires){
        return (
            <table className='table'>
                <thead>
                    <tr>
                        <th>questionnaire</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {
                        questionnaires.map(q => 
                            <tr>
                                <td>{q.name}</td>
                                <td> <Link to={"/respondquestionnaire/" + q.name }  >Add Response </Link> </td>
                            </tr>
                        )
                    }
                </tbody>
            </table>
        );
    }

    render(){
        
            let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : ListQuestionnaires.renderQuestionnaires(this.state.questionnaires);
            return (
                <div>
                    <h1>Questionnaires</h1>
                    {contents}
                </div>
            );
       
    }
}