import React from 'react';
import gql from "graphql-tag";
import {Query} from "react-apollo";
import TodoEdit from "./TodoEdit";

const GET_TODO = gql`
    query getTodo($todoId: String!) {
        todos(todoId: $todoId){
            id
            description
            complete
        }
    }
`;

const TodoDetails = (props) => (
    <Query query={GET_TODO} variables={{todoId: props.match.params.id}}>
        {({loading, error, data}) => {
            if (loading) return <p>Loading...</p>;
            if (error) return <p>Error :(</p>;
            const todo = data.todos[0];            
            return <TodoEdit id={todo.id} description={todo.description} complete={todo.complete} />;
        }}
    </Query>
);

export default TodoDetails;