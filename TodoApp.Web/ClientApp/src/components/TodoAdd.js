import React from 'react';
import gql from "graphql-tag";
import { Mutation } from "react-apollo";

const ADD_TODO = gql`
  mutation createTodo($todo: TodoInput!) {
    createTodo(todo: $todo) {
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

const TodoAdd = () => {
    let input;

    return (
        <Mutation 
            mutation={ADD_TODO} 
            update={(cache, {data: {createTodo}}) => {
                const { todos } = cache.readQuery({query: GET_TODOS});
                todos.push(createTodo); 
                cache.writeQuery({
                    query: GET_TODOS,
                    data: {todos: todos}
                })
            }}>
            {(createTodo, { data }) => (
                <div>
                    <form
                        onSubmit={e => {
                            e.preventDefault();
                            createTodo({ variables: { todo: {
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
                        />
                        <button type="submit">Add Todo</button>
                    </form>
                </div>
            )}
        </Mutation>
    );
};

export default TodoAdd;