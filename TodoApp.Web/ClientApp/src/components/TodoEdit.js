import React, {Component} from 'react';
import gql from "graphql-tag";
import { Mutation } from "react-apollo";
import {withRouter} from "react-router";

const EDIT_TODO = gql`
  mutation editTodo($todo: TodoUpdateInput!) {
    updateTodo(todo: $todo) {
      id
      description
      complete
    }
  }`;

const GET_TODOS = gql`
  query getTodos {
    todos {
      id
      description
      complete
    }
  }`;

class TodoEdit extends Component {
    
    constructor(props) {
        super(props);
        
        this.state = {
            id: this.props.id,
            description: this.props.description,
            complete: this.props.complete
        }
    }
    
    handleChange = (e) => {
        console.log(e);
        this.setState({description: e.target.value});
    };
    
    goBack = (e) => {
        this.props.history.push("/");
    };
    
    render() {
        let input;
        return (
            <Mutation
                mutation={EDIT_TODO}
                update={(cache, {data: {updateTodo}}) => {
                    const {todos} = cache.readQuery({query: GET_TODOS});

                    let todo = todos.filter(x => x.id === updateTodo.id)[0] ;
                    todo.description = updateTodo.description;

                    cache.writeQuery({
                        query: GET_TODOS,
                        data: {todos: todos}
                    })
                }}>
                {(updateTodo, {data}) => (
                    <div>
                        <form
                            onSubmit={e => {
                                e.preventDefault();
                                updateTodo({
                                    variables: {
                                        todo: {
                                            id: this.state.id,
                                            description: input.value,
                                            complete: this.state.complete
                                        }
                                    }
                                });
                                input.value = "";
                                this.goBack();
                            }}
                        >
                            <input
                                ref={node => {
                                    input = node;
                                }}
                                value={this.state.description}
                                onChange={this.handleChange}
                            />
                            <button type="submit">Add Todo</button>
                            <button type="button" onClick={this.goBack}>go back</button>
                        </form>
                    </div>
                )}
            </Mutation>
        );
    }
}

export default withRouter(TodoEdit);