import "./App.css";
import Content from "./Components/Content";
import Top from "./Components/Top";
import AddList from "./Components/AddList";
import requestHandler from "./Components/requestHandler";
import { useState, useEffect } from "react";

function App() {
  const API_url = "http://localhost:3500/players";

  const [list, setList] = useState([]);
  const [newItem, setNewItem] = useState("");
  const [editItem, setEditItem] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch(API_url);
        if (!response.ok) throw Error("Error Message");
        const listItem = await response.json();
        setList(listItem);
      } catch (error) {
        console.error(error);
      }
    };
    fetchData();
  }, []);

  const addItems = async (player) => {
    const id = list.length ? list[list.length - 1].id + 1 : 1;
    const theNewItem = { id, checked: false, player };
    const listItem = [...list, theNewItem];
    setList(listItem);

    const postOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(theNewItem),
    };

    const result = await requestHandler(API_url, postOptions);
    if (result) console.error(result);
  };

  const handleCheck = (id) => {
    const playerToEdit = list.find((player) => player.id === id);
    setEditItem(playerToEdit);
    setNewItem(playerToEdit.player);
  };

  const handleUpdatePlayer = async (e) => {
    e.preventDefault();
    if (editItem) {
      const updatedPlayer = { ...editItem, player: newItem };
      const updatedList = list.map((item) => (item.id === editItem.id ? updatedPlayer : item));
      setList(updatedList);

      const putOptions = {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(updatedPlayer),
      };

      const reqUrl = `${API_url}/${editItem.id}`;
      const result = await requestHandler(reqUrl, putOptions);
      if (result) console.error(result);

      setEditItem(null);
      setNewItem("");
    }
  };

  const handleDelete = async (id) => {
    const listItem = list.filter((player) => player.id !== id);
    setList(listItem);

    const deleteOptions = { method: "DELETE" };
    const reqUrl = `${API_url}/${id}`;
    const result = await requestHandler(reqUrl, deleteOptions);
    if (result) console.error(result);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (editItem) {
      handleUpdatePlayer(e);
    } else {
      addItems(newItem);
      setNewItem("");
    }
  };

  return (
    <div className="App">
      <Top />
      <AddList newItem={newItem} setNewItem={setNewItem} handleSubmit={handleSubmit} />
      <Content list={list} handleCheck={handleCheck} handleDelete={handleDelete} />
      
    </div>
  );
}

export default App;
