import * as React from "react";
import { Pivot, PivotItem } from "office-ui-fabric-react/lib/Pivot";
import { buttonHeaderProps } from "./UserProfileStyles";
import { Stack } from "@fluentui/react";
import { groupListItem, ItemsList } from "./GroupsList";

export const UserProfile: React.FunctionComponent = () => {
  return (
    <Stack horizontalAlign="center">
      <Pivot styles={{ root: { marginBottom: "2rem" } }}>
        <PivotItem
          headerButtonProps={{
            text: "Groups I was sign in into",
            styles: buttonHeaderProps,
          }}
        >
          <ItemsList originalItems={itemsForExmples} />
        </PivotItem>
        <PivotItem
          headerButtonProps={{
            text: "Groups created by me",
            styles: buttonHeaderProps,
          }}
        >
          <ItemsList originalItems={itemsForExmples} />
        </PivotItem>
        <PivotItem
          headerButtonProps={{
            text: "Groups I'm interested in",
            styles: buttonHeaderProps,
          }}
        >
          <ItemsList originalItems={itemsForExmples} />
        </PivotItem>
      </Pivot>
    </Stack>
  );
};

const itemForExmple: groupListItem = {
  link: "ynet.co.il",
  imageUrl: "https://bstore.bezeq.co.il/media/20696/740-2-blue.jpg",
  name: "Lenovo ThinkPad T4800",
};

const itemsForExmples: groupListItem[] = [
  {
    link: "ynet.co.il",
    imageUrl: "https://bstore.bezeq.co.il/media/20696/740-2-blue.jpg",
    name: "Lenovo ThinkPad T4800",
  },
];

for (let i = 0; i < 40; i++) {
  itemsForExmples.push(itemForExmple);
}
