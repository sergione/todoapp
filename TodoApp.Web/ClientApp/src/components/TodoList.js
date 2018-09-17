import React from 'react';
import { Query } from "react-apollo";
import gql from "graphql-tag";
import TodoListItem from "./TodoListItem";

const GET_TODOS = gql`
          {
            todos {
              id
              description  
              complete
            }
          }
        `;

const TodoList = () => (
    <Query query={GET_TODOS}>
        {({ loading, error, data }) => {
            if (loading) return <p>Loading...</p>;
            if (error) return <p>Error :(</p>;

            return data.todos.map(({ id, description, complete }) => (
                <TodoListItem key={id} id={id} checked={complete} description={description}/>
            ));
        }}
    </Query>
);

export default TodoList;