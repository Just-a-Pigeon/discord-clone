
ALTER TABLE server_nodes DROP CONSTRAINT fk_server_nodes_server_nodes_parent_id;

ALTER TABLE server_nodes ALTER COLUMN parent_id DROP NOT NULL;

ALTER TABLE server_nodes ADD CONSTRAINT fk_server_nodes_server_nodes_parent_id FOREIGN KEY (parent_id) REFERENCES server_nodes (id);
