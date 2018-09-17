import React from 'react';
import gql from "graphql-tag";
import { Mutation } from "react-apollo";

const EDIT_TODO = gql`
  mutation editTodo($todo: TodoInput!) {
    editTodo(todo: $todo) {
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

const TodoEdit = (props) => {
    let input;

    return (
        <Mutation 
            mutation={EDIT_TODO} 
            update={(cache, {data: {editTodo}}) => {
                const { todos } = cache.readQuery({query: GET_TODOS});

                //TODO:update todo
                
                cache.writeQuery({
                    query: GET_TODOS,
                    data: {todos: todos}
                })
            }}>
            {(editTodo, { data }) => (
                <div>
                    <form
                        onSubmit={e => {
                            e.preventDefault();
                            editTodo({ variables: { todo: {
                                        description: input.value,
                                        complete: false
                                    } } });
                            input.value = "";
                        }}
                    >
                        <input
                            ref={node => {
                                input = node;
                            }}
                            value={props.description}
                        />
                        <button type="submit">Add Todo</button>
                    </form>
                </div>
            )}
        </Mutation>
    );
};

export default TodoEdit;