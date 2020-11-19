import React from "react";
import * as styles from "./ProductPageStyles";
import { Image, Separator, Stack, Text } from "@fluentui/react";

export const ProductPage: React.FunctionComponent = () => {
  return (
    <Stack>
      <Stack horizontal>
        <Image
          src="https://bstore.bezeq.co.il/media/20696/740-2-blue.jpg"
          height="30rem"
          width="30rem"
        ></Image>
        <Separator vertical />
        <Stack>
          <Text
            className="Bold"
            styles={styles.headerStyle}
            variant="xLargePlus"
          >
            Product Name
          </Text>
          <Separator />
          <Text>Maximum Acceptable Price: 140 ₪</Text>
          <Text>Group's expiration date: {new Date().toString()}</Text>
          <Separator />
          <Text variant="large">Description </Text>
          <Text>
            This example shows how components that used to be styled using CSS
            can be styled using JS styling. (Look at the bottom of the code to
            see the equivalent SCSS.) The preferred method is JS styling for
            several reasons: type safety for styling, more predictable behavior,
            and clear feedback via typing when component changes affect existing
            styling code.
          </Text>
        </Stack>
      </Stack>
    </Stack>
  );
};
