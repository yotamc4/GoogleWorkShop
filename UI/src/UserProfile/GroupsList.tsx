import * as React from "react";
import { TextField } from "office-ui-fabric-react/lib/TextField";
import { Image, ImageFit } from "office-ui-fabric-react/lib/Image";
import { classNames } from "./GroupListStyles";
import { Stack } from "@fluentui/react";

export const ItemsList: React.FunctionComponent<{
  originalItems: groupListItem[];
}> = (props) => {
  const [items, setItems] = React.useState<groupListItem[]>(
    props.originalItems
  );

  const resultCountText =
    items.length === props.originalItems.length
      ? ""
      : ` (${items.length} of ${props.originalItems.length} shown)`;

  const onFilterChanged = (
    event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    setItems(
      props.originalItems.filter(
        (item) =>
          item.name
            .toLowerCase()
            .indexOf((event.target as HTMLInputElement).value.toLowerCase()) >=
          0
      )
    );
  };

  return (
    <Stack tokens={{ childrenGap: "1rem" }}>
      <TextField
        label={"Filter by name" + resultCountText}
        onChange={onFilterChanged}
      />
      <div className={classNames.list}>
        {items.map((item) => (
          <div className={classNames.itemCell} data-is-focusable={true}>
            <Image
              className={classNames.itemImage}
              src={item?.imageUrl}
              width={50}
              height={50}
              imageFit={ImageFit.cover}
            />
            <div className={classNames.itemContent}>
              <div className={classNames.itemName}>{item?.name}</div>
            </div>
          </div>
        ))}
      </div>
    </Stack>
  );
};

export interface groupListItem {
  link: string;
  imageUrl: string;
  name: string;
}
